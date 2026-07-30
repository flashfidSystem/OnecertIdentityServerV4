using OnecertApiV1.Vault.Models;

namespace OnecertApiV1.Vault
{
    public interface IHashiCorpVaultService
    {
        /// <summary>Authenticates to Vault with the configured client certificate and returns a client token, using an in-memory cache.</summary>
        Task<string> GetTokenAsync(CancellationToken cancellationToken = default);

        /// <summary>Reads the current username/password from Vault's static database credentials endpoint, using an in-memory cache. Proactively rotates first if the cached credential's TTL is below Vault:DaysBeforeAutoRotate.</summary>
        Task<DatabaseCredential> ReadDatabaseCredentialAsync(CancellationToken cancellationToken = default);

        /// <summary>Calls Vault's rotate-role endpoint to rotate the static role's password immediately. Returns false (without throwing) if the endpoint is not configured or the call fails - rotation failure is non-fatal since the current credential remains valid until its TTL expires.</summary>
        Task<bool> RotateDatabaseCredentialAsync(CancellationToken cancellationToken = default);

        /// <summary>Reads the existing SQL connection string from configuration and returns it with the User ID/Password replaced by Vault-issued credentials.</summary>
        Task<string> GetSqlConnectionStringAsync(CancellationToken cancellationToken = default);

        /// <summary>Reads the existing SQL connection string from configuration and returns it with the User ID/Password replaced by the given credential, without touching Vault or the credential cache.</summary>
        string BuildConnectionString(DatabaseCredential credential);
    }
}
