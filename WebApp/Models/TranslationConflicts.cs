namespace TranslateWebApp.Models
{
    public class Candidate
    {
        public string RawText { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int BlockId { get; set; }
        public int CheckRaw { get; set; }
        public bool Discarded { get; set; }
        public bool Approved { get; set; }
    }

    public class TextConflict
    {
        #region POCO

        public int WorkId { get; set; }
        public string RowKey { get; set; } = string.Empty;
        public string SrcText { get; set; } = string.Empty;
        public bool Flagged { get; set; }
        public List<Candidate> Candidate { get; set; } = new();

        #endregion
        #region Methods 

        public int CandidatesLeft => Candidate.Count(c => (!c.Discarded && !c.Approved));

        public int DiscardBlock(int blockId)
        {
            var block = Candidate.Find(e => e.BlockId == blockId);
            if (block != null)
                block.Discarded = true;
            return CandidatesLeft;
        }

        public int PickBlock(int blockId)
        {
            foreach (var c in Candidate)
            {
                if (c.BlockId == blockId)
                    c.Approved = true;
                else
                    c.Discarded = true;
            }
            return CandidatesLeft;
        }

        #endregion
    }

    public class TranslationConflicts
    {
        public List<TextConflict> Items { get; set; } = new();
    }

}
