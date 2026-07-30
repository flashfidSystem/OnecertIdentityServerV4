using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using OnecertApiV1.Vault.Models;

namespace OnecertApiV1.Vault
{
    public class HashiCorpVaultService : IHashiCorpVaultService
    {
        public const string AuthClientName = "HashiCorpVaultAuthClient";
        public const string ApiClientName = "HashiCorpVaultApiClient";

        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly VaultOptions _options;
        private readonly ILogger<HashiCorpVaultService> _logger;

        private readonly SemaphoreSlim _tokenLock = new(1, 1);
        private string? _cachedToken;
        private DateTimeOffset _cachedTokenExpiresAt = DateTimeOffset.MinValue;

        private readonly SemaphoreSlim _credentialLock = new(1, 1);
        private DatabaseCredential? _cachedCredential;
        private DateTimeOffset _cachedCredentialExpiresAt = DateTimeOffset.MinValue;

        public HashiCorpVaultService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IOptions<VaultOptions> options,
            ILogger<HashiCorpVaultService> logger)
        {
            _logger = logger;
            _logger.LogInformation("[HashiCorpVaultService] Constructor called.");
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _options = options.Value;
            _logger.LogInformation("[HashiCorpVaultService] Vault options loaded. Enabled={Enabled}, LoginUrl={LoginUrl}, RoleName={RoleName}, CertPath={CertPath}",
                _options.Enabled, _options.LoginUrl, _options.RoleName, _options.ClientCertificatePath);
        }

        public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[GetTokenAsync] Entering. CachedTokenExists={Cached}, CacheValid={Valid}",
                _cachedToken is not null, DateTimeOffset.UtcNow < _cachedTokenExpiresAt);

            if (_cachedToken is not null && DateTimeOffset.UtcNow < _cachedTokenExpiresAt)
            {
                _logger.LogInformation("[GetTokenAsync] Returning cached token.");
                return _cachedToken;
            }

            _logger.LogInformation("[GetTokenAsync] Acquiring token lock...");
            await _tokenLock.WaitAsync(cancellationToken);
            _logger.LogInformation("[GetTokenAsync] Token lock acquired.");
            try
            {
                if (_cachedToken is not null && DateTimeOffset.UtcNow < _cachedTokenExpiresAt)
                {
                    _logger.LogInformation("[GetTokenAsync] Token re-cached after lock. Returning.");
                    return _cachedToken;
                }

                _logger.LogInformation("[GetTokenAsync] Validating LoginUrl...");
                if (string.IsNullOrWhiteSpace(_options.LoginUrl))
                {
                    _logger.LogError("[GetTokenAsync] LoginUrl is empty.");
                    throw new InvalidOperationException("Vault:LoginUrl is not configured.");
                }

                _logger.LogInformation("[GetTokenAsync] Validating RoleName...");
                if (string.IsNullOrWhiteSpace(_options.RoleName))
                {
                    _logger.LogError("[GetTokenAsync] RoleName is empty.");
                    throw new InvalidOperationException("Vault:RoleName is not configured.");
                }

                _logger.LogInformation("[GetTokenAsync] Authenticating to HashiCorp Vault at {LoginUrl}", _options.LoginUrl);

                _logger.LogInformation("[GetTokenAsync] Creating AuthClient from IHttpClientFactory...");
                var client = _httpClientFactory.CreateClient(AuthClientName);
                _logger.LogInformation("[GetTokenAsync] AuthClient created. BaseAddress={BaseAddress}", client.BaseAddress);

                _logger.LogInformation("[GetTokenAsync] Sending POST to {LoginUrl} with RoleName={RoleName}", _options.LoginUrl, _options.RoleName);
                using var response = await client.PostAsJsonAsync(_options.LoginUrl, new { name = _options.RoleName }, JsonOptions, cancellationToken);
                _logger.LogInformation("[GetTokenAsync] Response received. StatusCode={StatusCode}", response.StatusCode);

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation("[GetTokenAsync] Response body length: {Length}", json?.Length ?? 0);

                if (!response.IsSuccessStatusCode)
                {
                    var error = TryParseError(json);
                    _logger.LogError("[GetTokenAsync] Vault login failed. Status={StatusCode}, Error={Error}", response.StatusCode, error);
                    throw new InvalidOperationException($"HashiCorp Vault login failed ({response.StatusCode}): {error}");
                }

                var tokenResponse = JsonSerializer.Deserialize<HashiCorpTokenResponse>(json, JsonOptions);
                var token = tokenResponse?.Auth?.ClientToken;

                if (string.IsNullOrWhiteSpace(token))
                {
                    _logger.LogError("[GetTokenAsync] No client token in response.");
                    throw new InvalidOperationException("HashiCorp Vault login response did not contain a client token.");
                }

                _cachedToken = token;
                _cachedTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(_options.TokenLifetimeMinutes);
                _logger.LogInformation("[GetTokenAsync] Token obtained and cached. ExpiresAt={ExpiresAt}", _cachedTokenExpiresAt);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetTokenAsync] Exception thrown. Type={ExceptionType}, Message={Message}", ex.GetType().Name, ex.Message);
                throw;
            }
            finally
            {
                _tokenLock.Release();
                _logger.LogInformation("[GetTokenAsync] Token lock released.");
            }
        }

        public async Task<DatabaseCredential> ReadDatabaseCredentialAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[ReadDatabaseCredentialAsync] Entering. CachedCred={Cached}, CacheValid={Valid}",
                _cachedCredential is not null, DateTimeOffset.UtcNow < _cachedCredentialExpiresAt);

            if (_cachedCredential is not null && DateTimeOffset.UtcNow < _cachedCredentialExpiresAt)
            {
                _logger.LogInformation("[ReadDatabaseCredentialAsync] Returning cached credential.");
                return _cachedCredential;
            }

            _logger.LogInformation("[ReadDatabaseCredentialAsync] Acquiring credential lock...");
            await _credentialLock.WaitAsync(cancellationToken);
            _logger.LogInformation("[ReadDatabaseCredentialAsync] Credential lock acquired.");
            try
            {
                if (_cachedCredential is not null && DateTimeOffset.UtcNow < _cachedCredentialExpiresAt)
                {
                    _logger.LogInformation("[ReadDatabaseCredentialAsync] Credential re-cached after lock. Returning.");
                    return _cachedCredential;
                }

                _logger.LogInformation("[ReadDatabaseCredentialAsync] Fetching credential from Vault...");
                var credential = await FetchCredentialFromVaultAsync(cancellationToken);
                _logger.LogInformation("[ReadDatabaseCredentialAsync] Credential fetched. Username={User}, TTL={Ttl}s", credential.Username, credential.TtlSeconds);

                var rotateThresholdSeconds = Math.Max(_options.DaysBeforeAutoRotate, 0) * 86400L;
                _logger.LogInformation("[ReadDatabaseCredentialAsync] RotateThreshold={Threshold}s, CredentialTTL={Ttl}s",
                    rotateThresholdSeconds, credential.TtlSeconds);

                if (rotateThresholdSeconds > 0 && credential.TtlSeconds < rotateThresholdSeconds)
                {
                    _logger.LogInformation("[ReadDatabaseCredentialAsync] TTL below threshold, rotating...");
                    var rotated = await RotateDatabaseCredentialAsync(cancellationToken);
                    if (rotated)
                    {
                        _logger.LogInformation("[ReadDatabaseCredentialAsync] Rotation succeeded, re-fetching...");
                        credential = await FetchCredentialFromVaultAsync(cancellationToken);
                        _logger.LogInformation("[ReadDatabaseCredentialAsync] Re-fetched after rotation. Username={User}, TTL={Ttl}s", credential.Username, credential.TtlSeconds);

                        if (credential.TtlSeconds < rotateThresholdSeconds)
                        {
                            _logger.LogWarning("[ReadDatabaseCredentialAsync] TTL still below threshold after rotation.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("[ReadDatabaseCredentialAsync] Rotation failed, continuing with current credential.");
                    }
                }

                _cachedCredential = credential;
                var cacheLifetimeSeconds = rotateThresholdSeconds > 0
                    ? Math.Max(credential.TtlSeconds - rotateThresholdSeconds, 0)
                    : Math.Max(credential.TtlSeconds - 30, 0);
                _cachedCredentialExpiresAt = DateTimeOffset.UtcNow.AddSeconds(cacheLifetimeSeconds);
                _logger.LogInformation("[ReadDatabaseCredentialAsync] Credential cached. CacheExpiresAt={ExpiresAt}", _cachedCredentialExpiresAt);

                return credential;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReadDatabaseCredentialAsync] Exception thrown. Type={ExceptionType}, Message={Message}", ex.GetType().Name, ex.Message);
                throw;
            }
            finally
            {
                _credentialLock.Release();
                _logger.LogInformation("[ReadDatabaseCredentialAsync] Credential lock released.");
            }
        }

        public async Task<bool> RotateDatabaseCredentialAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[RotateDatabaseCredentialAsync] Entering.");
            if (string.IsNullOrWhiteSpace(_options.RotateCredentialsUrl))
            {
                _logger.LogWarning("[RotateDatabaseCredentialAsync] RotateCredentialsUrl not configured, skipping.");
                return false;
            }

            try
            {
                _logger.LogInformation("[RotateDatabaseCredentialAsync] Getting token...");
                var token = await GetTokenAsync(cancellationToken);
                _logger.LogInformation("[RotateDatabaseCredentialAsync] Token obtained.");

                _logger.LogInformation("[RotateDatabaseCredentialAsync] Creating API client...");
                var client = _httpClientFactory.CreateClient(ApiClientName);
                using var request = new HttpRequestMessage(HttpMethod.Post, _options.RotateCredentialsUrl);
                request.Headers.Add("X-Vault-Token", token);

                _logger.LogInformation("[RotateDatabaseCredentialAsync] Sending POST to {Url}", _options.RotateCredentialsUrl);
                using var response = await client.SendAsync(request, cancellationToken);
                _logger.LogInformation("[RotateDatabaseCredentialAsync] Response received. StatusCode={StatusCode}", response.StatusCode);

                if (response.StatusCode == HttpStatusCode.NoContent || response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("[RotateDatabaseCredentialAsync] Rotation succeeded.");
                    return true;
                }

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var error = TryParseError(json);
                _logger.LogError("[RotateDatabaseCredentialAsync] Rotation failed. Status={StatusCode}, Error={Error}", response.StatusCode, error);
                return false;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "[RotateDatabaseCredentialAsync] Exception thrown. Type={ExceptionType}", ex.GetType().Name);
                return false;
            }
        }

        private async Task<DatabaseCredential> FetchCredentialFromVaultAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[FetchCredentialFromVaultAsync] Entering.");
            if (string.IsNullOrWhiteSpace(_options.ReadCredentialsUrl))
            {
                _logger.LogError("[FetchCredentialFromVaultAsync] ReadCredentialsUrl not configured.");
                throw new InvalidOperationException("Vault:ReadCredentialsUrl is not configured.");
            }

            _logger.LogInformation("[FetchCredentialFromVaultAsync] Getting token...");
            var token = await GetTokenAsync(cancellationToken);
            _logger.LogInformation("[FetchCredentialFromVaultAsync] Token obtained.");

            _logger.LogInformation("[FetchCredentialFromVaultAsync] Creating API client...");
            var client = _httpClientFactory.CreateClient(ApiClientName);

            using var request = new HttpRequestMessage(HttpMethod.Get, _options.ReadCredentialsUrl);
            request.Headers.Add("X-Vault-Token", token);

            _logger.LogInformation("[FetchCredentialFromVaultAsync] Sending GET to {Url}", _options.ReadCredentialsUrl);
            using var response = await client.SendAsync(request, cancellationToken);
            _logger.LogInformation("[FetchCredentialFromVaultAsync] Response received. StatusCode={StatusCode}", response.StatusCode);

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("[FetchCredentialFromVaultAsync] Response body length: {Length}", json?.Length ?? 0);

            if (!response.IsSuccessStatusCode)
            {
                var error = TryParseError(json);
                _logger.LogError("[FetchCredentialFromVaultAsync] Read failed. Status={StatusCode}, Error={Error}", response.StatusCode, error);
                throw new InvalidOperationException($"HashiCorp Vault credential read failed ({response.StatusCode}): {error}");
            }

            var credentialResponse = JsonSerializer.Deserialize<HashiCorpReadPasswordResponse>(json, JsonOptions);
            var data = credentialResponse?.Data;

            if (data is null || string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password))
            {
                _logger.LogError("[FetchCredentialFromVaultAsync] Response missing username/password. HasData={HasData}, HasUsername={HasUser}, HasPassword={HasPass}",
                    data is not null, !string.IsNullOrWhiteSpace(data?.Username), !string.IsNullOrWhiteSpace(data?.Password));
                throw new InvalidOperationException("HashiCorp Vault credential response did not contain a username/password.");
            }

            var credential = new DatabaseCredential
            {
                Username = data.Username,
                Password = data.Password,
                TtlSeconds = data.Ttl,
                FetchedAt = DateTimeOffset.UtcNow
            };
            _logger.LogInformation("[FetchCredentialFromVaultAsync] Credential parsed. Username={User}, TTL={Ttl}s", credential.Username, credential.TtlSeconds);
            return credential;
        }

        public async Task<string> GetSqlConnectionStringAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[GetSqlConnectionStringAsync] Entering.");
            var credential = await ReadDatabaseCredentialAsync(cancellationToken);
            _logger.LogInformation("[GetSqlConnectionStringAsync] Credential obtained, building connection string...");
            var connString = BuildConnectionString(credential);
            _logger.LogInformation("[GetSqlConnectionStringAsync] Connection string built.");
            return connString;
        }

        public string BuildConnectionString(DatabaseCredential credential)
        {
            _logger.LogInformation("[BuildConnectionString] Entering. Username={User}", credential.Username);
            var existingConnectionString = _configuration.GetConnectionString(_options.ConnectionStringName);
            if (string.IsNullOrWhiteSpace(existingConnectionString))
            {
                _logger.LogError("[BuildConnectionString] Connection string '{Name}' not found.", _options.ConnectionStringName);
                throw new InvalidOperationException($"Connection string '{_options.ConnectionStringName}' was not found in configuration.");
            }

            var connectionStringBuilder = new SqlConnectionStringBuilder(existingConnectionString)
            {
                UserID = credential.Username,
                Password = credential.Password,
                IntegratedSecurity = false
            };

            _logger.LogInformation("[BuildConnectionString] Done.");
            return connectionStringBuilder.ConnectionString;
        }

        private static string TryParseError(string json)
        {
            try
            {
                var error = JsonSerializer.Deserialize<HashiCorpErrorResponse>(json, JsonOptions);
                return error?.Errors is { Count: > 0 } ? string.Join(" || ", error.Errors) : json;
            }
            catch (JsonException)
            {
                return json;
            }
        }
    }
}
