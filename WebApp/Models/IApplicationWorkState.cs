
namespace TranslateWebApp.Models
{
    public interface IApplicationWorkState
    {
        bool ReadyToWork { get; }
        bool Disabled { get; }
        int CallsConflicts { get; }
        int CallsTranslations { get;  }
        List<WorkItem> Translations { get; }
        TranslationConflicts Conflicts { get; }
        void SetConflicts(TranslationConflicts conflicts);
        void SetTranslations(List<WorkItem> translations);
        bool ShowSettings { get; set; }
        void SetDisabled();
    }
}