using System.Text;
using System.Text.Json.Nodes;
using SwagCrawler.Analysis;
using SwagCrawler.Schema;

namespace SwagCrawler.Generation;

public class TypeGenerator
{
    private readonly DependencyGraph _graph;

    public TypeGenerator(DependencyGraph graph)
    {
        _graph = graph;
    }

    public ExportType Generate(SchemaItem schema, bool formatted)
    {
        string end = formatted ? "\n" : "";
        string typeName = schema.Name;
        var export = new ExportType
        {
            FileName = typeName + ".ts",
        };

        var dependencies = schema.RefIds;
        StringBuilder sb = new();
        foreach (string dep in dependencies.Reverse())
        {
            string temp = dep[2..];
            sb.Append($"import {{ {Path.GetFileName(temp)} }} from '{temp}';{end}");
        }

        sb.Append($"export type {typeName} = {{{end}");
        string indent = formatted ? "  " : "";
        foreach ((string key, JsonObject value) in schema.Properties)
        {
            SetTypeNotation(key, value, sb, indent);
            SetType(value, sb, indent, formatted);
        }

        sb.Append($"}};{end}");


        export.Source = sb.ToString();
        return export;
    }

    private void SetType(JsonObject value, StringBuilder sb, string indent, bool formatted)
    {
        string jump = formatted ? "\n" : "";
        string nextIndent = indent;
        nextIndent += formatted ? indent : "";
        var hasType = value.TryGetPropertyValue("type", out JsonNode? node);
        if (!hasType)
        {
            sb.Append($"never;{jump}");
            return;
        }
        switch (node!.GetValue<string>())
        {
            case "integer":
            {
                sb.Append($"number;{jump}{indent}");
                return;
            }
            case "string":
            {
                sb.Append($"string;{jump}{indent}");
                return;
            }
            case "boolean":
            {
                sb.Append($"boolean;{jump}{indent}");
                return;
            }
            case "object":
            case "array":
            {
                sb.Append($"{{/* code gen */}};{jump}");
                return;
            }
        }
    }

    private void SetTypeNotation(string key, JsonObject value, StringBuilder sb, string format)
    {
        bool hasNode = value.TryGetPropertyValue("nullable", out JsonNode? reaOnlyNode);
        bool isTrue = reaOnlyNode?.GetValue<bool>() ?? false;
        if (hasNode && isTrue)
        {
            sb.Append($"{format}readonly ");
        }
        else
        {
            sb.Append(format);
        }

        sb.Append(key);
        hasNode = value.TryGetPropertyValue("nullable", out JsonNode? node);
        isTrue = node?.GetValue<bool>() ?? false;
        if (hasNode && isTrue)
        {
            sb.Append('?');
        }

        sb.Append(": ");
    }
}