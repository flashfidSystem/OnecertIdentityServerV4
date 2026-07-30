using System.Text.Json.Serialization;

namespace OnecertApiV1.Vault.Models
{
    /// <summary>Equivalent of the VB "jsonHashiCorpReadPassword" class - response body of the static-creds read call.</summary>
    public class HashiCorpReadPasswordResponse
    {
        [JsonPropertyName("request_id")]
        public string? RequestId { get; set; }

        [JsonPropertyName("lease_id")]
        public string? LeaseId { get; set; }

        [JsonPropertyName("renewable")]
        public bool Renewable { get; set; }

        [JsonPropertyName("lease_duration")]
        public int LeaseDuration { get; set; }

        [JsonPropertyName("data")]
        public HashiCorpCredentialData? Data { get; set; }

        [JsonPropertyName("wrap_info")]
        public object? WrapInfo { get; set; }

        [JsonPropertyName("warnings")]
        public object? Warnings { get; set; }

        [JsonPropertyName("auth")]
        public object? Auth { get; set; }

        [JsonPropertyName("mount_type")]
        public string? MountType { get; set; }
    }
}
