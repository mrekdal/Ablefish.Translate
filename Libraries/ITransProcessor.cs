
namespace TransService
{
    public interface ITransProcessor
    {
        Task<string> Translate(ITranslatable translatable, string targetLangCode);
        Task<string> Translate(string originalText, string sourceLangCode, string targetLangCode);
    }

}