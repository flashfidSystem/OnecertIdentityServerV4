using System.Text.Json.Serialization;

namespace OnecertApiV1.Vault.Models
{
    /// <summary>Equivalent of the VB "jsonHashiCorpError" class.</summary>
    public class HashiCorpErrorResponse
    {
        [JsonPropertyName("errors")]
        public List<string>? Errors { get; set; }
    }
}
