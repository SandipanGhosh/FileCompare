using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace FileCompare
{
    public abstract class Search
    {
        public String FilePath { get; set; }
        public List<string> SearchTexts { get; set; }
        public string ReportPath { get; set; }
        public abstract void SearchTextInFile();

        public void GenerateSearchReport(Dictionary<string, int> dSearchPatterns)
        {
            string reportFileName = Path.GetFileNameWithoutExtension(FilePath);
            reportFileName += "_search.html";
            string reportFilePath = Path.Combine(ReportPath, reportFileName);
            Logger.Info(string.Format("Search::GenerateSearchReport - Writing search results to the file - {0} ...", reportFileName));

            try
            {
                if (File.Exists(reportFilePath))
                {
                    File.Delete(reportFilePath);
                }

                using (StreamWriter sw = File.CreateText(reportFilePath))
                {
                    string reportText = "<!DOCTYPE html>";
                    reportText += "<html>";
                    reportText += "<head><style>";
                    reportText += "table, th, td {border: 1px solid black;}";
                    reportText += "</style></head>";
                    reportText += "<body>";
                    reportText += "<h2>FileCompare - Search Report</h2>";
                    reportText += "<table table-layout: fixed; border-collapse: collapse; border: 3px solid purple;><tr><th>#</th><th>Pattern</th><th>Frequency</th></tr>";

                    List<string> kPatterns = dSearchPatterns.Keys.ToList();
                    int count = 0;
                    foreach(string pattern in kPatterns)
                    {
                        count++;
                        reportText += "<tr>";
                        reportText += "<td>";
                        reportText += count.ToString();
                        reportText += "</td>";
                        reportText += "<td>";
                        reportText += pattern;
                        reportText += "</td>";
                        reportText += "<td>";
                        reportText += dSearchPatterns[pattern].ToString();
                        reportText += "</td>";
                        reportText += "</tr>";
                    }
                    reportText += "</table></body></html>";
                    sw.WriteLine(reportText);
                }
            }
            catch(Exception ex)
            {
                Logger.Error(string.Format("Search::GenerateSearchReport - {0}", ex.ToString()));
            }
        }
    }
}
