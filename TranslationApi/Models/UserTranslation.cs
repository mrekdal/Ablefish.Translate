namespace TranslationApi.Models;

public class UserTranslation
{
    public string RowKey { get; set; } = string.Empty;
    public string TextOriginal { get; set; } = string.Empty;
    public string TextResX { get; set; } = string.Empty;
    public string TextTranslated { get; set; } = string.Empty;
    public int Changed { get; set; }
    public string LogTo { get; set; } = string.Empty;
}
