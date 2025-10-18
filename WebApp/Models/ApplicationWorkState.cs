namespace TranslateWebApp.Models
{
    public class ApplicationWorkState : IApplicationWorkState
    {
        public int CallsConflicts { get; internal set; }
        public int CallsTranslations { get; internal set; }
        public List<WorkItem> Translations { get; internal set; } = new();
        public TranslationConflicts Conflicts { get; internal set; } = new();

        public void SetConflicts(TranslationConflicts conflicts)
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
