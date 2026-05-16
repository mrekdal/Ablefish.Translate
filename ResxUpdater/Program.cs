using CommandLine;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace ResxUpdater;

public class Options
{
    [Option('p', "ProjectId", Required = true, HelpText = "Project ID to fetch translations for.")]
    public int ProjectId { get; set; }

    [Option('l', "LangKey", Required = true, HelpText = "Language key (e.g. 'de', 'fr').")]
    public string LangKey { get; set; } = string.Empty;

    [Option('u', "UserName", Required = true, HelpText = "LogTo user name.")]
    public string UserName { get; set; } = string.Empty;

    [Option('f', "File", Required = true, HelpText = "Full path to the .resx file to update.")]
    public string ResxFile { get; set; } = string.Empty;
}

public record UserTranslation(
    string RowKey,
    string TextOriginal,
    string TextResX,
    string TextTranslated,
    int Changed,
    string LogTo);

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        int exitCode = 0;

        await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(async options =>
            {
                exitCode = await Run(options, config);
            });

        return exitCode;
    }

    static async Task<int> Run(Options options, IConfiguration config)
    {
        string? baseUrl = config["TranslationApi:BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            Console.Error.WriteLine("ERROR: TranslationApi:BaseUrl is not configured.");
            return 1;
        }

        if (!File.Exists(options.ResxFile))
        {
            Console.Error.WriteLine($"ERROR: Resx file not found: {options.ResxFile}");
            return 1;
        }

        // Fetch translations from the API
        using HttpClient http = new HttpClient { BaseAddress = new Uri(baseUrl) };
        string url = $"/GetUserTranslation?projectId={options.ProjectId}&langKey={Uri.EscapeDataString(options.LangKey)}&userName={Uri.EscapeDataString(options.UserName)}";

        List<UserTranslation>? translations;
        try
        {
            translations = await http.GetFromJsonAsync<List<UserTranslation>>(url);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"ERROR: API call failed: {ex.Message}");
            return 1;
        }

        if (translations == null || translations.Count == 0)
        {
            Console.WriteLine("No translations returned.");
            return 0;
        }

        // Load the resx file preserving formatting
        XDocument doc = XDocument.Load(options.ResxFile, LoadOptions.PreserveWhitespace);
        XElement root = doc.Root ?? throw new InvalidOperationException("Invalid resx: no root element.");

        int updated = 0;
        int skipped = 0;

        foreach (UserTranslation t in translations)
        {
            if (string.IsNullOrWhiteSpace(t.TextTranslated))
            {
                skipped++;
                continue;
            }

            XElement? dataElement = root.Elements("data")
                .FirstOrDefault(e => (string?)e.Attribute("name") == t.RowKey);

            if (dataElement == null)
            {
                Console.WriteLine($"  SKIP (not found in resx): {t.RowKey}");
                skipped++;
                continue;
            }

            XElement? valueElement = dataElement.Element("value");
            if (valueElement == null)
            {
                Console.WriteLine($"  SKIP (no <value> element): {t.RowKey}");
                skipped++;
                continue;
            }

            valueElement.Value = t.TextTranslated;
            updated++;
            Console.WriteLine($"  UPDATED: {t.RowKey}");
        }

        doc.Save(options.ResxFile);

        Console.WriteLine();
        Console.WriteLine($"Done. Updated: {updated}, Skipped: {skipped}");
        return 0;
    }
}
