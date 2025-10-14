namespace TranslateWebApp.Models
{
    public class UserProject
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;

    }
    public class UserProjectStatus : UserProject
    {
        public int WorkTotal { get; set; }
        public int WorkDone { get; set; }

        public double WorkDonePercent { get => (double)WorkDone / WorkTotal * 100; }

        public string LangKey { get; set; } = string.Empty;

        public void AddOne()
        {
            WorkDone++;
        }

    }
}
