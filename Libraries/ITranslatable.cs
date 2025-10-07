using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransService
{
    public interface ITranslatable
    {
        string RowKey { get; }
        string SrcText { get; }
        string LangCode { get; }
        int CheckTxt { get; }
    }

}
