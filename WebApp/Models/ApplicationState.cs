namespace TranslateWebApp.Models
{
    public class ApplicationState : IApplicationState
    {
        public AppStage Stage { get; internal set; } = AppStage.Initiaizing;
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
        public bool ReadyToWork => (CallsTranslations > 0 && Stage == AppStage.DataLoaded);
        public bool Disabled => CallsTranslations < 0;

        public bool BusyState
        {
            get
            {
                return Stage <= AppStage.Loading ||
                       Stage == AppStage.UserLoaded;
            }
        }

        public void SetStage(AppStage appStage)
        {
            Stage = appStage;
            if (Stage == AppStage.LoadFailed)
            {
                CallsTranslations = 1; // Indicate that translations need to be loaded
            }
        }
    }
}
