using System.Text.Json;
using System.Text.Json.Nodes;
using SwagCrawler.Analysis;
using SwagCrawler.Generation;

namespace SwagCrawler;

public static class Program
{
    public static void Main(string[] args)
    {
        string file = args[0];
        FileStream stream = File.OpenRead(file);
        var obj = JsonSerializer.Deserialize<JsonObject>(stream);
        stream.Close();
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

        TypeGenerator gen = new(graph);
        var current = graph.First();
        foreach (GraphNode node in graph)
        {
            Console.WriteLine("Compiling {0}:", node.Name);
            node.type = gen.Generate(node.Schema, true);
            using StreamWriter writer = File.CreateText($"out/{node.type.FileName}");
            var imports = node.type?.Imports ?? new List<string>();
            for (int i = imports.Count; 0 < i; i--)
            {
                writer.WriteLine(imports[i]);
            }
            
            writer.WriteLine(node.type.Source);
        }
    }
}

