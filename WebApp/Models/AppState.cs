namespace TranslateWebApp.Models
{
    public class AppState : IAppState
    {
        public int CallsConflicts { get; internal set; }
        public int CallsTranslations { get; internal set; }
        public List<WorkItem> Translations { get; internal set; } = new();
        public Disagreements Conflicts { get; internal set; } = new();

        public void SetConflicts(Disagreements conflicts)
        {
            Conflicts = conflicts;
            CallsConflicts++;
        }
        public void SetTranslations(List<WorkItem> translations)
        {
            Translations = translations;
            CallsTranslations++;
        }
    }
}
