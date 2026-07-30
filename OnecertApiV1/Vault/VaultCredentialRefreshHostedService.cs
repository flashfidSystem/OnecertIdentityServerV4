using Microsoft.Extensions.Options;

namespace OnecertApiV1.Vault
{
    /// <summary>
    /// Periodically re-reads the database credential from Vault and refreshes
    /// ConnectionStrings:{ConnectionStringName} in configuration. Vault's static-creds roles rotate
    /// their password on their own rotation_period independently of this app; without this service
    /// the connection string fetched once at startup would go stale and every SQL login would start
    /// failing after the first rotation. The wait between checks adapts to the credential's actual
    /// TTL (capped by RefreshPollIntervalMinutes, floored at 30s) so a short TTL is still caught in
    /// time instead of only being checked on a fixed clock.
    /// </summary>
    public sealed class VaultCredentialRefreshHostedService : BackgroundService
    {
        private readonly IHashiCorpVaultService _vaultService;
        private readonly IConfiguration _configuration;
        private readonly VaultOptions _options;
        private readonly ILogger<VaultCredentialRefreshHostedService> _logger;

        public VaultCredentialRefreshHostedService(
            IHashiCorpVaultService vaultService,
            IConfiguration configuration,
            IOptions<VaultOptions> options,
            ILogger<VaultCredentialRefreshHostedService> logger)
        {
            _vaultService = vaultService;
            _configuration = configuration;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_options.Enabled)
            {
                return;
            }

            // Floor so a very short (or zero/unset) TTL from Vault can't turn this into a hot loop.
            var minDelay = TimeSpan.FromSeconds(30);
            var defaultDelay = TimeSpan.FromMinutes(Math.Max(_options.RefreshPollIntervalMinutes, 1));

            while (!stoppingToken.IsCancellationRequested)
            {
                var nextDelay = defaultDelay;

                try
                {
                    // Fetch the credential once and build the connection string from it directly
                    // (rather than also calling GetSqlConnectionStringAsync, which would re-check the
                    // cache and could trigger a redundant second Vault call for a very short TTL).
                    var credential = await _vaultService.ReadDatabaseCredentialAsync(stoppingToken).ConfigureAwait(false);
                    var connectionString = _vaultService.BuildConnectionString(credential);
                    _configuration[$"ConnectionStrings:{_options.ConnectionStringName}"] = connectionString;

                    // If the credential's actual remaining TTL is shorter than our normal poll
                    // cadence, check back sooner instead of blindly waiting the full interval and
                    // risking a window where the app keeps using an already-rotated (invalid)
                    // password. Uses RemainingTtl (not the raw TtlSeconds snapshot) since this
                    // credential may have come back from HashiCorpVaultService's own cache rather
                    // than a fresh fetch, and TtlSeconds alone would not reflect elapsed time.
                    // Note: RemainingTtl can legitimately be exactly zero (e.g. a rotation attempt
                    // just failed right at the wire) - that case must still shorten the delay down
                    // to minDelay, not fall through to the full default.
                    var untilExpiry = credential.RemainingTtl - TimeSpan.FromSeconds(30);
                    if (untilExpiry < TimeSpan.Zero)
                    {
                        untilExpiry = TimeSpan.Zero;
                    }

                    if (untilExpiry < nextDelay)
                    {
                        nextDelay = untilExpiry < minDelay ? minDelay : untilExpiry;
                    }
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Failed to refresh database credentials from HashiCorp Vault; continuing to use the last known-good connection string and retrying in {Delay}.", minDelay);
                    nextDelay = minDelay;
                }

                try
                {
                    await Task.Delay(nextDelay, stoppingToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
