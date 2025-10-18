
namespace TranslateWebApp.Models
{
    public interface IAppState
    {
        public int CallsConflicts { get; }
        public int CallsTranslations { get; }
        List<WorkItem> Translations { get; }
        Disagreements Conflicts { get; }
        public void SetConflicts(Disagreements conflicts);
        public void SetTranslations(List<WorkItem> translations);
    }
}