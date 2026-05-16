using CommandLine;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace AblefishResxUpdater
{
    public class Options
    {
        [Option('p', "ProjectId", Required = true, HelpText = "ProjectId to fetch translations for.")]
        public int ProjectId { get; set; }

        [Option('l', "LangKey", Required = true, HelpText = "Language key (e.g. 'es', 'nl', 'de', 'ca' or 'nb').")]
        public string LangKey { get; set; } = string.Empty;

        [Option('u', "UserName", Required = false, HelpText = "LogTo user name. If omitted, approved translations are fetched instead.")]
        public string UserName { get; set; } = string.Empty;

        [Option('f', "File", Required = true, HelpText = "Full path to the .resx file to update.")]
        public string ResxFile { get; set; } = string.Empty;
    }

    public class UserTranslation
    {
        public int RowId { get; set; }
        public string RowKey { get; set; } = string.Empty;
        public string TextOriginal { get; set; } = string.Empty;
        public string TextResX { get; set; } = string.Empty;
        public string TextTranslated { get; set; } = string.Empty;
        public int Changed { get; set; }
        public int BlockId { get; set; }
        public string LogTo { get; set; } = string.Empty;
    }

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
            // string? baseUrl = config["TranslationApi:BaseUrl"];
            string? baseUrl = "https://ablefish-api.azurewebsites.net";
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
            string url = string.IsNullOrWhiteSpace(options.UserName)
                ? $"/GetApprovedTranslation?projectId={options.ProjectId}&langKey={Uri.EscapeDataString(options.LangKey)}"
                : $"/GetUserTranslation?projectId={options.ProjectId}&langKey={Uri.EscapeDataString(options.LangKey)}&userName={Uri.EscapeDataString(options.UserName)}";

            List<UserTranslation>? translations;
            try
            {
                translations = await http.GetFromJsonAsync<List<UserTranslation>>(url);
                if (translations != null)
                    Console.Error.WriteLine($"\nSuccess: API call returned {translations.Count} translations.");
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
            int unchanged = 0;

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

                if (valueElement.Value == t.TextTranslated)
                {
                    unchanged++;
                    continue;
                }

                valueElement.Value = t.TextTranslated;
                updated++;
                Console.WriteLine($"  UPDATED: {t.RowKey}");
            }

            doc.Save(options.ResxFile);

            Console.WriteLine();
            Console.WriteLine($"DONE. Updated: {updated}, Skipped: {skipped}, Unchanged: {unchanged}");
            return 0;
        }
    }
}
