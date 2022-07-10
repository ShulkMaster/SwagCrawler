using System.Text.Json.Serialization;

namespace SwagCrawler.Schema;

public class SchemaProp
{
    public string Type { get; set; } = string.Empty;
    public bool Nullable { get; set; }
    public bool ReadOnly { get; set; }
    public int? MaxLength { get; set; }

    [JsonPropertyName("$ref")]
    public string RefId { get; set; } = string.Empty;
}