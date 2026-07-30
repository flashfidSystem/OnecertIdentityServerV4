namespace OnecertApiV1.Vault.Models
{
    /// <summary>Username/password pair retrieved from Vault's static database credentials endpoint.</summary>
    public class DatabaseCredential
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        /// <summary>Seconds until this credential's lease expires, as reported by Vault at the moment it was fetched.</summary>
        public int TtlSeconds { get; set; }

        /// <summary>UTC time this credential was fetched from Vault.</summary>
        public DateTimeOffset FetchedAt { get; set; }

        /// <summary>
        /// Actual remaining time-to-live right now, accounting for elapsed time since FetchedAt.
        /// TtlSeconds alone is a snapshot that does not shrink while this instance sits in a cache -
        /// use this instead of TtlSeconds for any scheduling/expiry decision.
        /// </summary>
        public TimeSpan RemainingTtl
        {
            get
            {
                var remaining = TimeSpan.FromSeconds(TtlSeconds) - (DateTimeOffset.UtcNow - FetchedAt);
                return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
            }
        }
    }
}
