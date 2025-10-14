using TranslateWebApp.Models;

namespace TranslateWebApp.Interfaces
{
    public interface IDataContext
    {
        Task<UserData> GetUserData(string logTo);
        Task ApproveAiText(WorkItem workItem);
        Task ApproveText(WorkItem workItem);
        Task StoreAiText(WorkItem workItem, string logTo);
        Task<List<WorkItem>> GetWorkBatch();
        public void SetProjectId(int projectId);
        Task<Disagreements> GetDisagreements(int projectId, string langCode);

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