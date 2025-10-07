using Microsoft.Extensions.Primitives;

namespace TransService
{

    public class TextTranslation: ITranslatable
    {
        public string RowKey { get; set; } = string.Empty;
        public string SrcText { get; set; } = string.Empty;
        public string LangCode { get; set; } = string.Empty;
        public int CheckTxt { get; set; }
    }

}
