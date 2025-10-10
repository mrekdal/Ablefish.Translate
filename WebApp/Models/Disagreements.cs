namespace TranslateWebApp.Models
{
    public class Candidate
    {
        public string RawText { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int BlockId { get; set; }
        public int CheckRaw { get; set; }
    }

    public class TextConflict
    {
        public int WorkId { get; set; }
        public string RowKey { get; set; } = string.Empty;
        public string SrcText { get; set; } = string.Empty;
        public List<Candidate> Candidate { get; set; } = new();
    }

    public class Disagreements
    {
        public List<TextConflict> Items { get; set; } = new();
    }

}
