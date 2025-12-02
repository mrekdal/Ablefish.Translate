
namespace TranslateWebApp.Models
{
    public enum AppStage
    {
        Initiaizing,
        Cleared,
        Loading,
        LoadFailed,
        UserLoaded,
        DataLoaded
    }

    public interface IApplicationState
    {
        bool BusyState { get; }
        bool ReadyToWork { get; }
        bool Disabled { get; }
        int CallsConflicts { get; }
        int CallsTranslations { get;  }
        List<WorkItem> Translations { get; }
        TranslationConflicts Conflicts { get; }
        void SetConflicts(TranslationConflicts conflicts);
        void SetTranslations(List<WorkItem> translations);
        bool ShowSettings { get; set; }
        void SetStage(AppStage appStage);
        AppStage Stage { get; }
    }
}