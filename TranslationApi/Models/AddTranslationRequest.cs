namespace TranslationApi.Models;

public class AddTranslationRequest
{
    public int ProjectId { get; set; }
    public string RowKey { get; set; } = string.Empty;
    public string LangKey { get; set; } = string.Empty;
    public string RawText { get; set; } = string.Empty;
    public int CheckSrc { get; set; }
    public string LogTo { get; set; } = string.Empty;
}
