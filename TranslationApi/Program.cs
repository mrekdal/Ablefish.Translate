using Dapper;
using Microsoft.Data.SqlClient;
using Scalar.AspNetCore;
using System.Data;
using TranslationApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/GetUserTranslation", async (int projectId, string langKey, string userName, IConfiguration config) =>
{
    string connectionString = config.GetConnectionString("MSSQL")
        ?? throw new InvalidOperationException("Connection string 'MSSQL' is not configured.");

    using IDbConnection connection = new SqlConnection(connectionString);
    var rows = await connection.QueryAsync<UserTranslation>(
        "EXEC API.GetUserTranslation @ProjectId, @LangKey, @UserName;",
        new { ProjectId = projectId, LangKey = langKey, UserName = userName });

    return Results.Ok(rows);
});

app.Run();
