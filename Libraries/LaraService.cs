using Lara.Sdk;
using Microsoft.Extensions.Configuration;

namespace TransService
{
    public class LaraService : ITransProcessor
    {
        private readonly string _laraAccessKeyId;
        private readonly string _laraAccessKeySecret;
        private readonly IConfiguration _config;
        private readonly Credentials _credentials;
        private readonly Dictionary<string, string> langMapper = new();

        public LaraService(IConfiguration config)
        {
            _config = config;
            _laraAccessKeyId = config.GetValue<string>("Lara:Key") ?? "";
            _laraAccessKeySecret = config.GetValue<string>("Lara:Secret") ?? "";
            _credentials = new Credentials(_laraAccessKeyId, _laraAccessKeySecret);
            langMapper.Add("bg", "bg-BG");
            langMapper.Add("ca", "ca-ES");
            langMapper.Add("da", "da-DK");
            langMapper.Add("en", "en-GB");
            langMapper.Add("es", "es-ES");
            langMapper.Add("fi", "fi-FI");
            langMapper.Add("fr", "fr-FR");
            langMapper.Add("hr", "hr-HR");
            langMapper.Add("nl", "nl-NL");
            langMapper.Add("it", "it-IT");
            langMapper.Add("nb", "nb-NO");
            langMapper.Add("pl", "pl-PL");
            langMapper.Add("sv", "sv-SE");
            langMapper.Add("tr", "tr-TR");
        }
        public async Task<string> Translate(ITranslatable translatable, string targetLangCode)
        {
            if (langMapper.TryGetValue(translatable.LangCode, out string? inputLanguage)
                && langMapper.TryGetValue(targetLangCode, out string? outputLanguage))
            {
                var lara = new global::Lara.Sdk.Translator(_credentials);

                // This translates your text from English ("en-US") to Italian ("it-IT").

                var result = await lara.Translate(translatable.SrcText, inputLanguage, outputLanguage);

                return result?.SingleTranslation ?? string.Empty;
            }
            else
                throw new InvalidDataException($"Unsupported source or target: {translatable.LangCode} / {targetLangCode}");
        }

        public async Task<string> Translate(string originalText, string sourceLangCode, string targetLangCode)
        {
            if (langMapper.TryGetValue(sourceLangCode, out string? inputLanguage)
                && langMapper.TryGetValue(targetLangCode, out string? outputLanguage))
            {
                var lara = new global::Lara.Sdk.Translator(_credentials);
                var result = await lara.Translate(originalText, inputLanguage, outputLanguage);
                return result?.SingleTranslation ?? string.Empty;
            }
            else
                throw new InvalidDataException($"Unsupported source or target: {sourceLangCode} / {targetLangCode}.");
        }
    }
}
