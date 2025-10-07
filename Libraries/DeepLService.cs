using DeepL;
using DeepL.Model;
using Microsoft.Extensions.Configuration;

namespace TransService
{
    public class DeepLService : ITransProcessor
    {
        private DeepLClient _client;

        public DeepLService(IConfiguration configuration)
        {
            _client = new DeepLClient(configuration.GetValue<string>("DeepL:AuthKey") ?? "");
        }

        public async Task<string> Translate(ITranslatable translatable, string targetLangCode)
        {
            TextResult? translatedText = await _client.TranslateTextAsync(translatable.SrcText, translatable.LangCode, targetLangCode);
            return translatedText.Text;
        }

        public async Task<string> Translate(string originalText, string sourceLangCode, string targetLangCode)
        {
            TextResult? translatedText = await _client.TranslateTextAsync(originalText, sourceLangCode, targetLangCode);
            return translatedText.Text;

        }
    }

}
