namespace SwagCrawler.Generation;

public class ExportType
{
    public string FileName { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public List<string> Imports { get; set; } = new();
}