using System.Text.Json.Serialization;

namespace OnecertApiV1.Vault.Models
{
    /// <summary>Equivalent of the VB "Auth" class.</summary>
    public class HashiCorpAuth
    {
        [JsonPropertyName("client_token")]
        public string? ClientToken { get; set; }

        [JsonPropertyName("accessor")]
        public string? Accessor { get; set; }

        [JsonPropertyName("policies")]
        public List<string>? Policies { get; set; }

        [JsonPropertyName("token_policies")]
        public List<string>? TokenPolicies { get; set; }

        [JsonPropertyName("metadata")]
        public HashiCorpTokenMetadata? Metadata { get; set; }

        [JsonPropertyName("lease_duration")]
        public int LeaseDuration { get; set; }

        [JsonPropertyName("renewable")]
        public bool Renewable { get; set; }

        [JsonPropertyName("entity_id")]
        public string? EntityId { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("orphan")]
        public bool Orphan { get; set; }

        [JsonPropertyName("mfa_requirement")]
        public object? MfaRequirement { get; set; }

        [JsonPropertyName("num_uses")]
        public int NumUses { get; set; }
    }
}
