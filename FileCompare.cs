using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace FileCompare
{
    public class FileCompare
    {
        static int Main(string[] args)
        {
            // Read the config params from the json file
            var vJsonString = File.ReadAllText("FileCompare.json");
            if (!(vJsonString.Length > 0))
            {
                //Logger.Error("Read from json failed!");
                return -1;
            }

            // Deserialize json
            var initConfig = JsonConvert.DeserializeObject<InitConfig>(vJsonString);

            // Validate config params
            if (false == initConfig.ValidateConfigParams())
            {
                //Logger.Error("Config param validation failed! Check inputs in FileCompare.json.");
                return -1;
            }

            // Initialize log4net
            string logFilePath = Path.Combine(initConfig.LogPath, initConfig.LogName);
            Logger.InitializeLogger(logFilePath);
            Logger.Info("FileCompare utility for pdf / doc comparison and pattern search ...");

            // Directory compare
            if (initConfig.Mode == 0)
            {
                Logger.Info("Initializing directory compare mode ...");

                QueryCompareDirs oCompareDirs = new QueryCompareDirs(initConfig.Directory1, initConfig.Directory2);
                oCompareDirs.CompareDirectories();
            }
            // Pattern search
            else if (initConfig.Mode == 1)
            {
                Logger.Info("Initializing search mode ...");
                Console.WriteLine("Initializing search mode ...");

                if (initConfig.PdfCompareSet.Count > 0)
                {
                    Logger.Info("Starting pdf search ...");
                    Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
                    String searchFilePath;
                    for (int i = 0; i < initConfig.PdfSearchSet.Count; i++)
                    {
                        dict = initConfig.PdfSearchSet[i];
                        searchFilePath = Path.Combine(initConfig.RootPath, dict["searchFile"][0].ToString());
                        Search oSearch = new PdfSearch(searchFilePath, dict["searchTexts"], dict["exceptions"], initConfig.ReportPath);
                        oSearch.SearchTextInFile();
                    }

                    Search.GenerateGlobalSearchReport(initConfig.ReportPath);
                    Search.GenerateGlobalExceptionReport(initConfig.ReportPath);
                }

                if (initConfig.DocCompareSet.Count > 0)
                {
                    Logger.Info("Starting doc search ...");
                    Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
                    String searchFilePath;
                    for (int i = 0; i < initConfig.DocSearchSet.Count; i++)
                    {
                        dict = initConfig.DocSearchSet[i];
                        searchFilePath = Path.Combine(initConfig.RootPath, dict["searchFile"][0].ToString());
                        Search oSearch = new DocSearch(searchFilePath, dict["searchTexts"], dict["exceptions"], initConfig.ReportPath);
                        oSearch.SearchTextInFile();
                    }

                    //Search.GenerateGlobalExceptionReport(initConfig.ReportPath);
                }
            }
            // File compare
            else if (initConfig.Mode == 2)
            {
                Logger.Info("Initializing compare mode ...");

                if (initConfig.PdfCompareSet.Count > 0)
                {
                    Logger.Info("Starting pdf compare ...");
                    Compare oCompare = new PdfCompare(initConfig.PdfCompareSet, initConfig.RootPath, initConfig.ReportPath);
                    oCompare.CompareFiles();
                }

                if (initConfig.DocCompareSet.Count > 0)
                {
                    Logger.Info("Starting doc compare ...");
                    Compare oCompare = new DocCompare(initConfig.DocCompareSet, initConfig.RootPath, initConfig.ReportPath);
                    oCompare.CompareFiles();
                }
            }

            return 0;
        }
    }
}
