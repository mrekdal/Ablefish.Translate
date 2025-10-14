namespace TranslateWebApp.Models
{
    public class UserProject
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;

    }
    public class UserProjectStatus: UserProject
    {
        public int WorkTotal { get; set; }
        public int WorkDone { get; set; }

        public decimal WorkTotalPercent { get => (decimal)(WorkDone / WorkTotal * 100.0); }

        public string LangKey { get; set; } = string.Empty;

    }
}
