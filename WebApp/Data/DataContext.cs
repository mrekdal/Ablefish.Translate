using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Data;
using TranslateWebApp.Models;

namespace TranslateWebApp.Data
{
    public class DataContext : IDataContext
    {

        private const int DEMO_PROJECT = 4;

        private UserData _userData = new();
        private List<TargetLanguage> _targetLanguages = new();

        // private string _sourceLanguage = "en";
        private string _connectionString = string.Empty;
        private IConfiguration _config;
        private ILogger<DataContext> _logger;
        public DataContext(ILogger<DataContext> logger, IConfiguration configuration)
        {
            _config = configuration;
            _connectionString = _config.GetConnectionString("MSSQL") ?? "";
            _logger = logger;
            _userData.Clear();
        }

        public int ProjectId { get => _userData.ProjectId; }
        public string HelperLanguage { get => _userData.HelperLanguage; set => _userData.HelperLanguage = value; }
        public string TargetLanguage { get => _userData.TargetLanguage; set => _userData.TargetLanguage = value; }

        public List<TargetLanguage> TargetLanguages { get => _targetLanguages;  }
        public void SetProjectId(int projectId)
        {
            _userData.ProjectId = projectId;
        }

        public async Task<List<WorkItem>> GetWorkBatch()
        {
            _logger.LogInformation($"EXEC dbo.GetWorkBatch( {_userData.ProjectId}, '{_userData.LogTo}', '{_userData.TargetLanguage}' );");
            string sql = $"EXEC dbo.GetWorkBatch @ProjectId, @LogTo, @TargetLanguage, @HelperLanguage;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<WorkItem>(sql, new { _userData.ProjectId, _userData.LogTo, _userData.TargetLanguage,_userData.HelperLanguage });
                return rows.ToList();
            }
        }

        public async Task ApproveAiText(WorkItem workItem)
        {
            workItem.WorkFinal = workItem.WorkAi;
            await ApproveText(workItem);
        }

        public async Task ApproveText(WorkItem workItem)
        {
            _logger.LogInformation($"EXEC dbo.ApproveFinalText( {workItem.WorkId}, {workItem.Src1Check} );");
            workItem.Approve();
            string sql = $"EXEC dbo.ApproveFinalText @WorkId, @LogTo, @TargetLanguage, @Src1Check, @WorkFinal;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { workItem.WorkId, _userData.LogTo, _userData.TargetLanguage, workItem.Src1Check, workItem.WorkFinal });
            }
        }

        public async Task StoreAiText(WorkItem workItem, string logToAi)
        {
            _logger.LogInformation($"EXEC dbo.AddTextBlock( {workItem.WorkId}, '{workItem.LangWorkKey}', '{logToAi}' );");
            string sql = $"EXEC dbo.AddTextBlock @WorkId, @LangWorkKey, @WorkAi, @LogTo;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { workItem.WorkId, workItem.LangWorkKey, workItem.WorkAi, LogTo = logToAi });
            }
        }

        public async Task<UserData> GetUserData(string logTo)
        {
            _logger.LogInformation($"GetUserData({logTo})");
            string sql = $"EXEC Web.GetUserFromLogTo @LogTo;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<UserData>(sql, new { LogTo = logTo });
                _userData = rows.FirstOrDefault<UserData>() ?? new();
            }
            sql = $"EXEC Web.GetTargets @LogTo;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<TargetLanguage>(sql, new { LogTo = logTo });
                _targetLanguages = rows.ToList<TargetLanguage>();
            }
            return _userData;
        }

    }
}
