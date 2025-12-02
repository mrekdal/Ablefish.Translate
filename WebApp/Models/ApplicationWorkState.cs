namespace TranslateWebApp.Models
{
    public class ApplicationWorkState : IApplicationWorkState
    {
        public bool ShowSettings { get; set; } = false;     
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
        public bool ReadyToWork => CallsTranslations > 0;
        public bool Disabled => CallsTranslations < 0;  

        public void SetDisabled()
        {
            CallsTranslations = -1;
        }
    }
}
