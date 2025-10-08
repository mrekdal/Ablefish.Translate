namespace TranslateWebApp.Models
{
    public class Language
    {
        public Language() { }
        public Language( string langKey, string fullName, string shortName)
        {
            LangKey = langKey;
            EnglishName = fullName;
            ShortName = shortName;
        }
        public string LangKey { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
    }
}
