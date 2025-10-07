using TranslateWebApp.Data;

namespace TranslateWebApp.Models
{
    public class WorkItem
    {
        private bool _approved;
        public int WorkId { get; set; }
        public string RowKey { get; set; } = string.Empty;

        #region Translation Source data

        public string Src1Key { get; set; } = string.Empty;
        public string Src1Text { get; set; } = string.Empty;
        public int Src1Check { get; set; }

        #endregion
        
        #region Translation Helper data

        public string Src2Key { get; set; } = string.Empty;
        public string Src2Text { get; set; } = string.Empty;
        public int Src2Check { get; set; }
        public bool Src2Machine { get; set; }

        #endregion
        
        #region Translation Machine data

        public string LangAiKey { get; set; } = string.Empty;
        public string WorkAi { get; set; } = string.Empty;
        public int WorkAiCheck { get; set; }

        #endregion
        
        #region Translation Manual data

        public string LangWorkKey { get; set; } = string.Empty;
        public string WorkFinal { get; set; } = string.Empty;
        public int WorkFinalCheck { get; set; }

        #endregion

        #region Translation Metadata

        public string WorkLanguage { get; set; } = string.Empty;
        public string HelpLanguage { get; set; } = string.Empty;
        public string LogTo { get; set; } = string.Empty;
        public int ApprId { get; set; }

        #endregion

        public bool IsApproved { get => _approved; }
        public void Approve()
        {
            _approved = true;
        }
        public string WorkMerged
        {
            get
            {
                if (string.IsNullOrEmpty(WorkFinal))
                    return WorkAi;
                else
                    return WorkFinal;
            }
        }
    }
}
