using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Data;
using System.Text.Json;
using TranslateWebApp.Interfaces;
using TranslateWebApp.Models;

namespace TranslateWebApp.Data
{
    public class DataContext : IDataContext
    {

        private UserData _userData = new();
        private List<Language> _targetLanguages = new();
        private List<Language> _supportLanguages = new();
        private List<UserProject> _userProjects = new();

        private string _connectionString = string.Empty;
        private IConfiguration _config;
        private ILogger<DataContext> _logger;

        public DataContext(ILogger<DataContext> logger, IConfiguration configuration)
        {
            _config = configuration;
            _connectionString = _config.GetConnectionString("MSSQL") ?? "";
            _logger = logger;
            _userData.Clear();
            _supportLanguages.Add(new Language("xx", "No Language", "-"));
            _supportLanguages.Add(new Language("ca", "Catalan", "CAT"));
            _supportLanguages.Add(new Language("nb", "Norwegian - Bokmål", "NOR"));
            _supportLanguages.Add(new Language("nl", "Dutch", "NED"));
            _supportLanguages.Add(new Language("es", "Spanish", "SPA"));
        }

        public int ProjectId { get => _userData.ProjectId; }
        public string HelperLanguage { get => _userData.HelperLanguage; set => _userData.HelperLanguage = value; }
        public string TargetLanguage { get => _userData.TargetLanguage; set => _userData.TargetLanguage = value; }

        public List<UserProject> UserProjects { get => _userProjects; }
        public List<Language> SupportLanguages { get => _supportLanguages; }
        public List<Language> TargetLanguages { get => _targetLanguages; }
        public void SetProjectId(int projectId)
        {
            _userData.ProjectId = projectId;
        }

        public async Task<List<WorkItem>> GetWorkBatch()
        {
            _logger.LogInformation($"EXEC Web.GetWorkBatch( {_userData.ProjectId}, '{_userData.LogTo}', '{_userData.TargetLanguage}' );");
            string sql = $"EXEC Web.GetWorkBatch @ProjectId, @LogTo, @TargetLanguage, @HelperLanguage;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<WorkItem>(sql, new { _userData.ProjectId, _userData.LogTo, _userData.TargetLanguage, _userData.HelperLanguage });
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
            _logger.LogInformation($"EXEC Web.ApproveFinalText( {workItem.WorkId}, {workItem.Src1Check} );");
            workItem.Approve();
            string sql = $"EXEC Web.ApproveFinalText @WorkId, @LogTo, @TargetLanguage, @Src1Check, @WorkFinal;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { workItem.WorkId, _userData.LogTo, _userData.TargetLanguage, workItem.Src1Check, workItem.WorkFinal });
            }
        }

        public async Task StoreAiText(WorkItem workItem, string logToAi)
        {
            _logger.LogInformation($"EXEC Web.AddTextBlock( {workItem.WorkId}, '{workItem.LangWorkKey}', '{logToAi}' );");
            string sql = $"EXEC Web.AddTextBlock @WorkId, @LangWorkKey, @WorkAi, @LogTo;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { workItem.WorkId, workItem.LangWorkKey, workItem.WorkAi, LogTo = logToAi });
            }
        }

        public async Task<UserData> GetUserData(string logTo)
        {
            _logger.LogInformation($"EXEC Web.GetUserData({logTo})");
            string sql = $"EXEC Web.GetUserFromLogTo @LogTo;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<UserData>(sql, new { LogTo = logTo });
                _userData = rows.FirstOrDefault<UserData>() ?? new();
            }
            sql = $"EXEC Web.GetTargets @LogTo;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<Language>(sql, new { LogTo = logTo });
                _targetLanguages = rows.ToList<Language>();
            }
            sql = $"EXEC Web.GetProjects @LogTo;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<UserProject>(sql, new { LogTo = logTo });
                _userProjects = rows.ToList<UserProject>();
            }
            return _userData;
        }

        public async Task<Disagreements> GetDisagreements(int projectId, string langCode)
        {
            Disagreements disagreements = new();
            _logger.LogInformation($"EXEC WebJson.GetDisagreements( {projectId}, '{langCode}';");
            string sql = $"EXEC WebJson.GetDisagreements @ProjectId, @LangKey;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string? jsonResult = await connection.ExecuteScalarAsync<string?>(sql, new { ProjectId = projectId, LangKey = langCode });
                if (jsonResult != null)
                    disagreements = JsonSerializer.Deserialize<Disagreements>(jsonResult) ?? new();
            }
            return disagreements;
        }
    }
}
