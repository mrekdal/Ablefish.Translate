using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransService
{
    internal class MonoTranslation : ITranslatable
    {
        public int MonographId { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string RowKey { get => $"M{MonographId}.{FieldName}"; }
        public string SrcText { get; set; } = string.Empty;
        public string LangCode { get; set; } = "en";
        public int CheckTxt { get; set; }
    }

}
