namespace OnecertApiV1.Vault
{
    /// <summary>
    /// Configuration for HashiCorp Vault cert-auth login and static database credential retrieval.
    /// Bound from the "Vault" section of appsettings.json.
    /// </summary>
    public class VaultOptions
    {
        public const string SectionName = "Vault";

        /// <summary>When false, the existing ConnectionStrings:SqlConnection value is used as-is and Vault is never called.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>Vault cert-auth role name (HashiCorpName in the VB config, e.g. "50610_global_app_role").</summary>
        public string RoleName { get; set; } = string.Empty;

        /// <summary>Vault cert-auth login endpoint (HashiCorpLogin).</summary>
        public string LoginUrl { get; set; } = string.Empty;

        /// <summary>Static database credentials read endpoint (HashiCorpReadPassword).</summary>
        public string ReadCredentialsUrl { get; set; } = string.Empty;

        /// <summary>Rotate-role endpoint (HashiCorpRotatePassword), called automatically when a read credential's TTL is below DaysBeforeAutoRotate.</summary>
        public string RotateCredentialsUrl { get; set; } = string.Empty;

        /// <summary>Filesystem path to the client certificate used for Vault cert auth (ServerCert).</summary>
        public string ClientCertificatePath { get; set; } = string.Empty;

        /// <summary>Optional password for the client certificate, if the .pfx is password protected.</summary>
        public string? ClientCertificatePassword { get; set; }

        /// <summary>
        /// When a read credential's remaining TTL (in days) drops below this, HashiCorpVaultService
        /// proactively calls RotateCredentialsUrl and re-reads the freshly rotated credential
        /// (mirrors the VB app's DaysB4AutoRotate). Set to 0 or less to disable auto-rotation.
        /// </summary>
        public int DaysBeforeAutoRotate { get; set; } = 10;

        /// <summary>How long a Vault token is trusted before re-authenticating (mirrors the VB app's 475-minute cache window).</summary>
        public int TokenLifetimeMinutes { get; set; } = 475;

        /// <summary>Name of the connection string in ConnectionStrings whose User ID/Password should be replaced with Vault-issued values.</summary>
        public string ConnectionStringName { get; set; } = "SqlConnection";

        /// <summary>
        /// How often the background refresh service checks whether the cached credential needs
        /// renewing. A Vault call only actually happens when the cached credential is near its TTL,
        /// so a short interval here is cheap - it just bounds how late a rotation can be noticed.
        /// </summary>
        public int RefreshPollIntervalMinutes { get; set; } = 5;
    }
}
