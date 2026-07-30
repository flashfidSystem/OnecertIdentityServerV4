using System.Text.Json.Serialization;

namespace OnecertApiV1.Vault.Models
{
    /// <summary>Equivalent of the VB "Data" class.</summary>
    public class HashiCorpCredentialData
    {
        [JsonPropertyName("last_vault_rotation")]
        public string? LastVaultRotation { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("rotation_period")]
        public int RotationPeriod { get; set; }

        [JsonPropertyName("ttl")]
        public int Ttl { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }
    }
}
