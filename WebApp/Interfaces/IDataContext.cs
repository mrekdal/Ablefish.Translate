using Ablefish.Blazor.Observer;
using Ablefish.Translation.WebApp.Models;

namespace Ablefish.Translation.WebApp.Interfaces
{
    public interface IDataContext
    {
        public bool IsLoaded { get; }   
        Action? OnUserDataChanged { get; set; }
        UserData UserData { get; }
        Task LoadUserData(string logTo);
        Task ApproveAiText(WorkItem workItem, bool withDoubt);
        Task ApproveText(WorkItem workItem, bool withDoubt);
        Task StoreAiText(WorkItem workItem, string logTo);
        Task LoadTranslations(string logTo);
        Task LoadTranslationsText(string logTo, string searchFor);  
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

        Task FlagWorkItem( int workId );
    }
}