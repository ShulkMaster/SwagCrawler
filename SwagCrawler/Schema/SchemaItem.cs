using System.Text.Json.Nodes;

namespace SwagCrawler.Schema;

public class SchemaItem
{
    private bool computed;

    public string Name { get; set; } = string.Empty;
    public bool AdditionalProperties { get; set; }
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, JsonObject> Properties { get; set; } = new();

    public HashSet<string> RefIds { get; } = new();

    public void ComputeProps()
    {
        if(computed) return;
        foreach (var prop in Properties.SelectMany(pair => pair.Value.AsObject()))
        {
            switch (prop.Key)
            {
                case "$ref":
                    RefIds.Add(prop.Value.ToString());
                    continue;
                case "items":
                {
                    string refId = prop.Value
                        .AsObject()["$ref"]
                        .GetValue<string>();
                    RefIds.Add(refId);
                    break;
                }
            }
        }

        computed = true;
    }
}