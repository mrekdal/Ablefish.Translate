#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

class Program
{
    static Dictionary<string, string> GetFieldMapping()
    {
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Description", "ChemicalDescription" },
            { "Character", "Characteristics" },
            { "Rationale", "Rationale" },
            { "SideEffects", "SideEffects" },
            { "Important", "ImportantInfo" },
            { "Preclinical", "PreclinicalData" },
            { "Personal", "PersonalCommunication" },
            { "Metabolism", "Metabolism" },
            { "Published", "PublishedExperience" },
            { "Hepatic", "HepaticExposure" },
            { "EpnetReports", "EpnetDrugReports" }
        };
    }

    static void Main(string[] args)
    {
        bool commit = args.Length > 0 && args[0].Equals("--commit", StringComparison.OrdinalIgnoreCase);

        // Load configuration
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        string? connectionString = config.GetConnectionString("Translate"); // use Translate
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Console.WriteLine("ERROR: Connection string 'Translate' not found.");
            Environment.Exit(1);
        }

        // Fetch data from FinalText
        var updates = new Dictionary<int, Dictionary<string, string>>();

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            using var cmd = new SqlCommand(
                "SELECT * FROM Web.GetApprovedTranslations( 3, 'es' );", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string rowKey = reader.GetString(1);
                string spanishText = reader.IsDBNull(2) ? "" : reader.GetString(2);

                var parts = rowKey.Split('.');
                if (parts.Length != 3)
                {
                    Console.WriteLine($"-- WARN: Invalid RowKey format: {rowKey}");
                    continue;
                }

                if (!int.TryParse(parts[1], out int monographId))
                {
                    Console.WriteLine($"-- WARN: Invalid MonographId in RowKey: {rowKey}");
                    continue;
                }

                string fieldName = parts[2];

                if (!updates.TryGetValue(monographId, out var fields))
                {
                    fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    updates[monographId] = fields;
                }

                if (fields.ContainsKey(fieldName))
                {
                    Console.WriteLine($"-- WARN: Duplicate field '{fieldName}' for MonographId {monographId}. Overwriting previous value.");
                }

                fields[fieldName] = spanishText;
            }
        }

        WriteSqlConsole(updates, commit, GetFieldMapping());
        WriteSummary(updates);
    }

    static void WriteSqlConsole(Dictionary<int, Dictionary<string, string>> updates, bool commit, Dictionary<string, string> fieldMapping)
    {
        Console.WriteLine("-- Auto-generated update script");
        Console.WriteLine("-- REVIEW CAREFULLY BEFORE RUNNING");
        Console.WriteLine($"-- Default behavior is {(commit ? "COMMIT" : "ROLLBACK")}");
        Console.WriteLine($"-- Generated on {DateTime.UtcNow:u}");
        Console.WriteLine();

        // Database name check in the SQL itself
        Console.WriteLine("IF DB_NAME() NOT LIKE '%Spanish%'");
        Console.WriteLine("BEGIN");
        Console.WriteLine("    RAISERROR('ERROR: This database does not contain \"Spanish\". Aborting.', 16, 1);");
        Console.WriteLine("    RETURN;");
        Console.WriteLine("END");
        Console.WriteLine();

        Console.WriteLine("BEGIN TRANSACTION;");
        Console.WriteLine();

        foreach (var monograph in updates)
        {
            Console.WriteLine($"-- MonographId {monograph.Key}");
            Console.WriteLine("UPDATE dbo.Monograph");
            Console.WriteLine("SET");

            int i = 0;
            foreach (var field in monograph.Value)
            {
                string escapedText = field.Value.Replace("'", "''");
                string comma = (++i < monograph.Value.Count) ? "," : "";
                string columnName = fieldMapping[field.Key];

                Console.WriteLine($"    [{columnName}] = N'{escapedText}'{comma}");
            }

            Console.WriteLine($"WHERE MonographId = {monograph.Key};");
            Console.WriteLine();
        }

        Console.WriteLine(commit ? "COMMIT TRANSACTION;" : "ROLLBACK TRANSACTION;");
        Console.WriteLine();
    }

    static void WriteSummary(Dictionary<int, Dictionary<string, string>> updates)
    {
        int totalMonographs = updates.Count;
        int totalFields = updates.Values.Sum(d => d.Count);

        Console.WriteLine("-- Summary:");
        Console.WriteLine($"-- Total MonographId updated: {totalMonographs}");
        Console.WriteLine($"-- Total fields updated: {totalFields}");

        var columnCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var fields in updates.Values)
        {
            foreach (var col in fields.Keys)
            {
                if (!columnCounts.ContainsKey(col)) columnCounts[col] = 0;
                columnCounts[col]++;
            }
        }

        Console.WriteLine("-- Updates per column:");
        foreach (var kv in columnCounts.OrderBy(k => k.Key))
        {
            Console.WriteLine($"--   {kv.Key}: {kv.Value}");
        }
    }
}
