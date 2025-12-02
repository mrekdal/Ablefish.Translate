using Ablefish.Blazor.Observer;
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

        private Guid ContextId = Guid.NewGuid();
        private UserData _userData = new();
        private IApplicationWorkState _appState;
        private List<Language> _targetLanguages = new();
        private List<Language> _supportLanguages = new();
        private List<UserProject> _userProjects = new();
        private List<UserProjectStatus> _userProjectStatus = new();
        private UserProjectStatus? _userProject;

        private string _connectionString = string.Empty;
        private IConfiguration _config;
        private ILogger<DataContext> _logger;
        private bool ValidUser
        {
            get => _userData.IsLoaded && _userData.UserId > 0;
        }

        public DataContext(ILogger<DataContext> logger, IConfiguration configuration, IApplicationWorkState appState)
        {
            _config = configuration;
            _appState = appState;
            _connectionString = _config.GetConnectionString("MSSQL") ?? "";
            _logger = logger;
            _userData.Clear(string.Empty);
            _supportLanguages.Add(new Language("xx", "No Language", "-"));
            _supportLanguages.Add(new Language("ca", "Catalan", "CAT"));
            _supportLanguages.Add(new Language("nb", "Norwegian - Bokmål", "NOR"));
            _supportLanguages.Add(new Language("nl", "Dutch", "NED"));
            _supportLanguages.Add(new Language("es", "Spanish", "SPA"));
        }

        public UserData UserData => _userData;
        public int ProjectId { get => _userData.ProjectId; }
        public string HelperLanguage { get => _userData.HelperLanguage; set => _userData.HelperLanguage = value; }
        public string TargetLanguage
        {
            get => _userData.TargetLanguage;
            set
            {
                _userData.TargetLanguage = value;
                UpdateProject();
            }
        }

        public List<UserProject> UserProjects { get => _userProjects; }
        public List<Language> SupportLanguages { get => _supportLanguages; }
        public List<Language> TargetLanguages { get => _targetLanguages; }

        private async Task<bool> CheckUser(string logTo)
        {
            await LoadUserData(logTo);
            if (!ValidUser)
            {
                _targetLanguages.Clear();
                _userProjects.Clear();
                _userProjectStatus.Clear();
                _userProject = null;
                return false;
            }
            return true;
        }

        private void UpdateProject()
        {
            _userProject = _userProjectStatus.Find(up => up.ProjectId == _userData.ProjectId && up.LangKey == _userData.TargetLanguage);
        }
        public void SetProjectId(int projectId)
        {
            _userData.ProjectId = projectId;
            UpdateProject();
        }

        public double PercentDone()
        {
            if (_userProject == null)
                return 0;
            else
                return _userProject.WorkDonePercent;
        }

        public async Task ApproveAiText(WorkItem workItem, bool withDoubt)
        {
            workItem.WorkFinal = workItem.WorkAi;
            await ApproveText(workItem, withDoubt);
        }

        public async Task ApproveText(WorkItem workItem, bool withDoubt)
        {
            _userProject?.AddOne();
            _logger.LogInformation($"EXEC Web.ApproveFinalText( {workItem.WorkId}, {workItem.Src1Check} );");
            workItem.Approve();
            string sql = $"EXEC Web.ApproveFinalText @WorkId, @LogTo, @TargetLanguage, @Src1Check, @WorkFinal, @withDoubt;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { workItem.WorkId, _userData.LogTo, _userData.TargetLanguage, workItem.Src1Check, workItem.WorkFinal, withDoubt });
            }
        }

        public async Task DiscardBlock(int blockId)
        {
            string sql = $"EXEC Web.DiscardBlock @BlockId, @LogTo;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { BlockId = blockId, _userData.LogTo });
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

        public async Task LoadUserData(string logTo)
        {
            if (logTo == string.Empty)
                _userData.Clear(logTo);
            else if (_userData.IsValid && _userData.LogTo.Equals(logTo))
                _logger.LogInformation($"GetUserData({logTo}): Returning cached object.");
            else if (_userData.IsLoaded)
                _logger.LogInformation($"GetUserData ({logTo}): Returning cached object.");
            else
                try
                {
                    _userData.Clear(logTo);
                    _logger.LogInformation($"EXEC Web.GetUserData({logTo}). Ctx: {ContextId}");
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
                    sql = $"EXEC Web.GetProjectStatus @LogTo;";
                    using (IDbConnection connection = new SqlConnection(_connectionString))
                    {
                        var rows = await connection.QueryAsync<UserProjectStatus>(sql, new { LogTo = logTo });
                        _userProjectStatus = rows.ToList<UserProjectStatus>();
                    }
                    _userData.SetLoaded();
                }
                catch (Exception ex)
                {
                    _appState.SetDisabled();
                    _userData.SetFailed(logTo, ex.Message);
                    _logger.LogError(ex, $"Error loading user data for {logTo}");
                }
        }

        public async Task LoadTranslations(string logTo)
        {
            if (await CheckUser(logTo) == false) return;
            _logger.LogInformation($"EXEC Web.GetWorkBatch( {_userData.ProjectId}, '{_userData.LogTo}', '{_userData.TargetLanguage}' );");
            string sql = $"EXEC Web.GetWorkBatch @ProjectId, @LogTo, @TargetLanguage, @HelperLanguage;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<WorkItem>(sql, new { _userData.ProjectId, _userData.LogTo, _userData.TargetLanguage, _userData.HelperLanguage });
                _appState.SetTranslations(rows.ToList());
            }
        }
        public async Task LoadTranslationsText(string logTo, string searchFor)
        {
            if (await CheckUser(logTo) == false) return;
            _logger.LogInformation($"EXEC Web.GetTextBatch( {_userData.ProjectId}, '{_userData.LogTo}', '{_userData.TargetLanguage}' );");
            string sql = $"EXEC Web.GetTextBatch @ProjectId, @LogTo, @TargetLanguage, @HelperLanguage, @SearchFor;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rows = await connection.QueryAsync<WorkItem>(sql, new { _userData.ProjectId, _userData.LogTo, _userData.TargetLanguage, _userData.HelperLanguage, searchFor });
                _appState.SetTranslations(rows.ToList());
            }
        }

        public async Task LoadConflicts(string logTo)
        {
            if (await CheckUser(logTo) == false) return;
            _logger.LogInformation($"EXEC WebJson.GetDisagreements( {_userData.ProjectId}, '{_userData.TargetLanguage}';");
            string sql = $"EXEC WebJson.GetDisagreements @ProjectId, @LangKey;";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string? jsonResult = await connection.ExecuteScalarAsync<string?>(sql, new { ProjectId = _userData.ProjectId, LangKey = _userData.TargetLanguage });
                if (jsonResult != null)
                    _appState.SetConflicts(JsonSerializer.Deserialize<TranslationConflicts>(jsonResult) ?? new());
            }
        }
    }
}
