using System.Text.Json;
using System.Text.Json.Nodes;
using SwagCrawler.Generation;
using SwagCrawler.Schema;

namespace SwagCrawler.Analysis;

public class GraphNode
{
    private readonly SchemaItem _json;
    public string RefIf { get; set; } = string.Empty;
    public ExportType? type;

    public GraphNode(JsonObject json, string name)
    {
        _json = JsonSerializer.Deserialize<SchemaItem>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
        Name = name;
        _json.Name = Name;
        _json.ComputeProps();
    }

    public string Name { get; }
    public SchemaItem Schema => _json;
    public bool Compiled => type is not null;
}