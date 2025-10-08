using Azure;
using Azure.AI.Translation.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransService
{
    public class AzureTranslate : ITransProcessor
    {

        private const string AZURE_ENDPOINT = "https://api.cognitive.microsofttranslator.com/";
        private TextTranslationClient client;

        public AzureTranslate(IConfiguration config)
        {
            var credential = new AzureKeyCredential(config.GetValue<string>("Azure:Key") ?? "KeyMissing");

            client = new TextTranslationClient(credential, new Uri(AZURE_ENDPOINT), config.GetValue<string>("Azure:Region") ?? "RegionMissing");
        }

        public async Task<string> Translate(ITranslatable translatable, string targetLangCode)
        {
            return await Translate(translatable.SrcText, translatable.LangCode, targetLangCode);
        }

        public async Task<string> Translate(string originalText, string sourceLangCode, string targetLangCode)
        {
            var response = await client.TranslateAsync(targetLangCode, originalText);
            if (response == null)
                throw new NullReferenceException("No response received.");
            else
            {
                foreach (var translation in response.Value)
                {
                    foreach (var t in translation.Translations)
                    {
                        return t.Text;
                    }
                }
                throw new NullReferenceException("Nothing found in enumeration");
            }
        }
    }
}
