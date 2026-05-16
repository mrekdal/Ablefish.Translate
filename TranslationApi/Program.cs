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
    if (langKey.Length < 2 || langKey.Length > 8)
        return Results.BadRequest("langKey must be between 2 and 8 characters.");

    string connectionString = config.GetConnectionString("MSSQL")
        ?? throw new InvalidOperationException("Connection string 'MSSQL' is not configured.");

    try
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        var rows = await connection.QueryAsync<UserTranslation>(
            "EXEC API.GetUserTranslation @ProjectId, @LangKey, @UserName;",
            new { ProjectId = projectId, LangKey = langKey, UserName = userName });

        if (!rows.Any())
            return Results.NoContent();

        return Results.Ok(rows);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    }
})
.WithName("GetUserTranslation")
.WithSummary("Get user translations")
.WithDescription("Returns all translations for the given project, language and user by calling the stored procedure API.GetUserTranslation.")
.Produces<IEnumerable<UserTranslation>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/GetApprovedTranslation", async (int projectId, string langKey, IConfiguration config) =>
{
    if (langKey.Length < 2 || langKey.Length > 8)
        return Results.BadRequest("langKey must be between 2 and 8 characters.");

    string connectionString = config.GetConnectionString("MSSQL")
        ?? throw new InvalidOperationException("Connection string 'MSSQL' is not configured.");

    try
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        var rows = await connection.QueryAsync<UserTranslation>(
            "EXEC API.GetApprovedTranslation @ProjectId, @LangKey;",
            new { ProjectId = projectId, LangKey = langKey });

        if (!rows.Any())
            return Results.NoContent();

        return Results.Ok(rows);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    }
})
.WithName("GetApprovedTranslation")
.WithSummary("Get approved translations")
.WithDescription("Returns all approved translations for the given project and language by calling the stored procedure API.GetApprovedTranslation.")
.Produces<IEnumerable<UserTranslation>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError);

app.Run();
