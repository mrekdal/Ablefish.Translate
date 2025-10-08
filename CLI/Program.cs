using Ablefish.StringUtils;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using PostTranslations.Models;
using System.Xml;
using System.Xml.Serialization;
using TransService;


namespace PostTranslations
{

    public class Options
    {
        [Option('e', "Engine", Required = false, Default ="Azure", HelpText = $"Translation engine (Azure, Lara, DeepL).")]
        public string Engine { get; set; } = string.Empty;
        [Option('m', "Monograph", Required = false, HelpText = $"Read data from monographs fields 1-n.")]
        public int Monograph { get; set; }
        [Option('r', "Resource", Required = false, HelpText = $"Read data from resource files.")]
        public string ResourceFile { get; set; } = string.Empty;
        [Option('t', "Target", Required = false, HelpText = $"Translate into target language.")]
        public string Target { get; set; } = string.Empty;
        [Option('p', "Project", Required = false, HelpText = $"Translate project data")]
        public int ProjectId { get; set; }
        [Option('n', "Count", Default = 100, Required = false, HelpText = $"Maximum number of translations.")]
        public int MaxCount { get; set; }
    }

    internal class Program
    {

        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // optional but good practice
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(options =>
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                if (options.Monograph > 0)
                    GetMonographData(config).GetAwaiter().GetResult();
                if (!string.IsNullOrEmpty(options.ResourceFile))
                    ReadResourceData(options.ResourceFile);
                if (!string.IsNullOrEmpty(options.Target))
                    DoTranslationWork(options.ProjectId, options.Target, options.MaxCount, config, options.Engine).GetAwaiter().GetResult();
            });
        }

        static async Task GetMonographData(IConfiguration config)
        {
            DataContext dc = new DataContext(config);
            var workList = await dc.GetMonographList();
            foreach (var e in workList)
                Console.WriteLine($"EXEC dbo.AddTextBlock 1, '{e.RowKey}', {SqlUtils.TextToSql(e.LangCode)}, {SqlUtils.TextToSql(e.SrcText)}, 'ResX';");
        }

        static void ReadResourceData(string sourceLanguage)
        {
            string origPath = @"C:\IKT_Tools\VisualStudio\work\Napos\DrugDatabase\WebApp\Locales\";
            string fullPathXmlFile = Path.Combine(origPath, $"Resource.{sourceLanguage}.resx");
            var xmlStream = File.OpenRead(fullPathXmlFile);
            if (xmlStream != null)
            {
                XmlReaderSettings settings = new XmlReaderSettings()
                {
                    DtdProcessing = DtdProcessing.Ignore,
                    MaxCharactersFromEntities = 1024
                };
                XmlReader xmlReader = XmlReader.Create(xmlStream, settings);
                XmlSerializer serializer = new XmlSerializer(typeof(ResourceFile));
                ResourceFile? originalFile = (ResourceFile?)serializer.Deserialize(xmlReader);
                if (originalFile != null)
                {
                    foreach (var e in originalFile.Data)
                    {
                        Console.WriteLine($"EXEC dbo.AddFinalText 1, '{e.Name}', {SqlUtils.TextToSql(sourceLanguage)}, {SqlUtils.TextToSql(e.Value)};");
                    }
                }
            }
        }

        static async Task DoTranslationWork(int projectId, string targetLanguage, int maxCount, IConfiguration config, string serviceName )
        {
            DataContext dc = new DataContext(config);
            TransFactory transFactory = new TransFactory(config);
            if (transFactory.TryGetService(serviceName, out ITransProcessor? service) && service != null)
            {
                var workList = await dc.GetWorklist(projectId, targetLanguage, serviceName);
                int stringCount = 0;
                foreach (var e in workList)
                {
                    if (e.SrcText.Trim().Length > 0)
                    {
                        string translation = await service.Translate(e, targetLanguage);
                        Console.WriteLine($"EXEC dbo.AddTextBlockRowKey {projectId}, '{e.RowKey}', '{targetLanguage}', {SqlUtils.TextToSql(translation)}, '{serviceName}';");
                        stringCount++;
                    }
                    if (stringCount == maxCount)
                        break;
                }
            }
        }
    }
}



