using System.Text.Json;
using System.Text.Json.Nodes;
using SwagCrawler.Analysis;

namespace SwagCrawler;

public static class Program
{
    public static void Main(string[] args)
    {
        string file = args[0];
        FileStream stream = File.OpenRead(file);
        var obj = JsonSerializer.Deserialize<JsonObject>(stream);
        JsonObject comps = obj["components"].AsObject();
        JsonObject schemas = comps["schemas"].AsObject();
        var graph = new DependencyGraph(schemas.Count);
        foreach (var key in schemas)
        {
            graph.Insert(new GraphNode(key.Value.AsObject(), key.Key)
            {
                RefIf = $"#/components/schemas/{key.Key}"
            });
        }

        Directory.CreateDirectory("out");
        
        foreach (GraphNode node in graph)
        {
            Console.WriteLine("{0}, depends on: ", node.Name);
            foreach (string refId in node.Schema.RefIds)
            {
                Console.WriteLine("\t{0}", refId);
            }
        }
    }
}

