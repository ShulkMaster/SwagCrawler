using System.Text;
using SwagCrawler.Schema;

namespace SwagCrawler.Generation;

public static class TypeGenerator
{

    public static string Generate(SchemaItem schema)
    {
        StringBuilder sb = new();
        string typeName = schema.Name;
        return typeName;
    }

}