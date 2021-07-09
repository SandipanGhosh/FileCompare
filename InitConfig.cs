using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCompare
{
    public class InitConfig
    {
        public int Mode { get; set; }
        public string RootPath { get; set; }
        public string ReportPath { get; set; }
        public string Directory1 { get; set; }
        public string Directory2 { get; set; }
        public string LogPath { get; set; }
        public string LogName { get; set; }
        public List<Dictionary<string, string>> PdfCompareSet { get; set; }
        public List<Dictionary<string, string>> DocCompareSet { get; set; }
        public List<Dictionary<string, List<string>>> PdfSearchSet { get; set; }
        public List<Dictionary<string, List<string>>> DocSearchSet { get; set; }

        public bool ValidateConfigParams()
        {
            // Validate Mode
            if (Mode != 0 & Mode != 1 & Mode != 2)
            {
                //Logger.Error("InitConfig::ValidateConfigParams - Mode validation failed! (Directory_Compare=0, Search=1, Compare=2)");
                return false;
            }

            // Validate RootPath
            if (!Directory.Exists(RootPath))
            {
                //Logger.Error("InitConfig::ValidateConfigParams - RootPath validation failed! Directory does not exist.");
                return false;
            }

            // Validate ReportPath
            if (!Directory.Exists(ReportPath))
            {
                //Logger.Error("InitConfig::ValidateConfigParams - ReportPath validation failed! Directory does not exist.");
                return false;
            }

            // Validate LogPath
            if (!Directory.Exists(LogPath))
            {
                //Logger.Error("InitConfig::ValidateConfigParams - LogPath validation failed! Directory does not exist.");
                return false;
            }

            // Validate LogName
            if (LogName.Length == 0)
            {
                //Logger.Error("InitConfig::ValidateConfigParams - LogName validation failed! LogName cannot be empty.");
                return false;
            }

            // Validate directory search params
            if (Mode == 0)
            {
                if (!Directory.Exists(Directory1) || !Directory.Exists(Directory2))
                {
                    //Logger.Error("InitConfig::ValidateConfigParams - Directory validation failed! Either of the directories does not exist.");
                    return false;
                }
            }

            // Validate search params
            if (Mode == 1)
            {
                if ((PdfSearchSet.Count == 0) & (DocSearchSet.Count == 0))
                {
                    //Loggger.Error("InitConfig::ValidateConfigParams - SearchSet validation failed! Either of PdfSearchSet or DocSearchSet should be non-empty.");
                    return false;
                }

                // Validate PdfSearchSet
                if (PdfSearchSet.Count != 0)
                {
                    Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
                    for (int i = 0; i < PdfSearchSet.Count; i++)
                    {
                        dict = PdfSearchSet[i];
                        if ((dict["searchFile"].Count == 0) || ((dict["searchTexts"].Count == 0) && (dict["exceptions"].Count == 0)))
                        {
                            //Logger.Error("InitConfig::ValidateConfigParams - Either of search file or search text and exception list is empty!");
                            return false;
                        }
                    }
                }

                // Validate DocSearchSet
                if (DocSearchSet.Count != 0)
                {
                    Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
                    for (int i = 0; i < DocSearchSet.Count; i++)
                    {
                        dict = DocSearchSet[i];
                        if ((dict["searchFile"].Count == 0) || ((dict["searchTexts"].Count == 0) && (dict["exceptions"].Count == 0)))
                        {
                            //Logger.Error("InitConfig::ValidateConfigParams - Either of search file or search text and exception list is empty!");
                            return false;
                        }
                    }
                }
            }

            // Validate compare params
            if (Mode == 2)
            {
                if ((PdfCompareSet.Count == 0) & (DocCompareSet.Count == 0))
                {
                    //Logger.Error("InitConfig::ValidateConfigParams - Compare set validation failed! Either of PdfCompareSet or DocCompareSet should be non-empty.");
                    return false;
                }
            }

            //Logger.Info("InitConfig::ValidateConfigParams - Config param validation passed.");
            return true;
        }
    }
}
