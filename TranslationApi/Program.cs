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

app.MapPost("/AddTranslation", async (AddTranslationRequest request, IConfiguration config) =>
{
    if (request.LangKey.Length < 2 || request.LangKey.Length > 12)
        return Results.BadRequest("LangKey must be between 2 and 12 characters.");

    if (string.IsNullOrWhiteSpace(request.RowKey))
        return Results.BadRequest("RowKey is required.");

    if (string.IsNullOrWhiteSpace(request.LogTo))
        return Results.BadRequest("LogTo is required.");

    string connectionString = config.GetConnectionString("MSSQL")
        ?? throw new InvalidOperationException("Connection string 'MSSQL' is not configured.");

    try
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync(
            "EXEC API.AddTextBlockRowKey @ProjectId, @RowKey, @LangKey, @RawText, @CheckSrc, @LogTo;",
            new
            {
                request.ProjectId,
                request.RowKey,
                request.LangKey,
                request.RawText,
                request.CheckSrc,
                request.LogTo
            });

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    }
})
.WithName("AddTranslation")
.WithSummary("Add or update a translation")
.WithDescription("Posts a translation for the given project, row key and language by calling the stored procedure API.AddTextBlockRowKey.")
.Accepts<AddTranslationRequest>("application/json")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError);

app.Run();
