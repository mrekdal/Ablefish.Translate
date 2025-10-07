using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TransService;

namespace PostTranslations.Models
{
    internal class DataContext
    {
        private string _translateConnection = string.Empty;
        private string _naposConnection = string.Empty;
        private string _databaseName = string.Empty;
        private IConfiguration _config;
        // private ILogger<DataContext> _logger;

        public DataContext( IConfiguration configuration)
        {
            _config = configuration;
            _translateConnection = _config.GetConnectionString("MSSQL") ?? "";
            _naposConnection = _config.GetConnectionString("Drugs") ?? "";
        }

        public async Task<List<TextTranslation>> GetWorklist( int projectId, string targetLangCode, string logTo )
        {
            string sql = @"EXEC dbo.GetWorklist @ProjectId, @LangCode, @LogTo;";
            using (IDbConnection connection = new SqlConnection(_translateConnection))
            {
                _databaseName = connection.Database;
                var rows = await connection.QueryAsync<TextTranslation>(sql, new { ProjectId = projectId, LangCode = targetLangCode, LogTo = logTo } );
                return rows.ToList();
            }
        }
        public async Task<List<TextTranslation>> GetMonographList()
        {
            string sql = @"EXEC XL.GetWorklist;";
            using (IDbConnection connection = new SqlConnection(_naposConnection))
            {
                _databaseName = connection.Database;
                var rows = await connection.QueryAsync<TextTranslation>(sql);
                return rows.ToList();
            }
        }

    }
}
