using System;
using System.IO;
using Newtonsoft.Json;

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
            Logger.Info("FileCompare utility for document comparison and pattern search ...");

            // Directory compare
            if (initConfig.Mode == 0)
            {
                Logger.Info("Initializing directory compare mode ...");
            }
            // Pattern search
            else if (initConfig.Mode == 1)
            {
                Logger.Info("Initializing search mode ...");
            }
            // File compare
            else if (initConfig.Mode == 2)
            {
                Logger.Info("Initializing compare mode ...");
            }

            return 0;
        }
    }
}
