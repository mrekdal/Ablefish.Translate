using Ablefish.Blazor.Observer;
using TranslateWebApp.Models;

namespace TranslateWebApp.Interfaces
{
    public interface IDataContext
    {
        UserData UserData { get; }
        Task LoadUserData(string logTo);
        Task ApproveAiText(WorkItem workItem);
        Task ApproveText(WorkItem workItem);
        Task StoreAiText(WorkItem workItem, string logTo);
        Task LoadTranslations(string logTo);
        Task LoadTranslationsText(string logTo, string searchFor );
        public void SetProjectId(int projectId);
        Task LoadConflicts(string logTo);

        public int ProjectId { get; }
        public double PercentDone();
        Task DiscardBlock(int blockId);

        public string HelperLanguage { get; set; }
        public string TargetLanguage { get; set; }
        List<Language> SupportLanguages { get; }
        List<Language> TargetLanguages { get; }
        List<UserProject> UserProjects { get; }
    }
}