using System.Text.Json.Serialization;

namespace OnecertApiV1.Vault.Models
{
    /// <summary>Equivalent of the VB "Metadata" class.</summary>
    public class HashiCorpTokenMetadata
    {
        [JsonPropertyName("authority_key_id")]
        public string? AuthorityKeyId { get; set; }

        [JsonPropertyName("cert_name")]
        public string? CertName { get; set; }

        [JsonPropertyName("common_name")]
        public string? CommonName { get; set; }

        [JsonPropertyName("serial_number")]
        public string? SerialNumber { get; set; }

        [JsonPropertyName("subject_key_id")]
        public string? SubjectKeyId { get; set; }
    }
}
