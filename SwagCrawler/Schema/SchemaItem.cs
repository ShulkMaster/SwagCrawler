using System.Text.Json.Nodes;

namespace SwagCrawler.Schema;

public class SchemaItem
{
    private bool computed;
    private List<string> _refIds = new();

    public string Name { get; set; } = string.Empty;
    public bool AdditionalProperties { get; set; }
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, JsonObject> Properties { get; set; } = new();

    public List<string> RefIds
    {
        get
        {
            if (!computed) ComputeProps();
            return _refIds;
        }
    }

    private void ComputeProps()
    {
        foreach (var prop in Properties.SelectMany(pair => pair.Value.AsObject()))
        {
            switch (prop.Key)
            {
                case "$ref":
                    _refIds.Add(prop.Value.ToString());
                    continue;
                case "items":
                {
                    string refId = prop.Value
                        .AsObject()["$ref"]
                        .GetValue<string>();
                    _refIds.Add(refId);
                    break;
                }
            }
        }
    }
}