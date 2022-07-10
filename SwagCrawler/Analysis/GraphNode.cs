using System.Text.Json;
using System.Text.Json.Nodes;
using SwagCrawler.Schema;

namespace SwagCrawler.Analysis;

public class GraphNode
{
    private readonly SchemaItem _json;
    public string RefIf { get; set; } = string.Empty;
    private bool compiled;

    public GraphNode(JsonObject json, string name)
    {
        _json = JsonSerializer.Deserialize<SchemaItem>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
        Name = name;
    }

    public string Name { get; }
    public SchemaItem Schema => _json;
}