
namespace TranslateWebApp.Models
{
    public interface IApplicationWorkState
    {
        public int CallsConflicts { get; }
        public int CallsTranslations { get; }
        List<WorkItem> Translations { get; }
        TranslationConflicts Conflicts { get; }
        public void SetConflicts(TranslationConflicts conflicts);
        public void SetTranslations(List<WorkItem> translations);
    }
}