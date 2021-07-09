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
        public List<string> Exceptions { get; set; }
        public string ReportPath { get; set; }
        public static string SearchReportText { get; set; }
        public static string ExceptionReportText { get; set; }
        public static string GlobalSearchReportText { get; set; }
        public static string GlobalExceptionReportText { get; set; }
        public abstract void SearchTextInFile();

        /* ----------------------------------------------------------------
         * Start:
         * Global search and exception report generation methods.
         * 
         * ------------------------------------------------------------- */

        public static void GenerateGlobalSearchReport(string reportPath)
        {
            DateTime dateTime = DateTime.Now;
            string reportFileName = "pdf_global_search_";
            reportFileName += dateTime.ToString("yyyyMMddHHmmss");
            reportFileName += ".html";
            string reportFilePath = Path.Combine(reportPath, reportFileName);
            Logger.Info(string.Format("Search::GenerateGlobalSearchReport - Writing global search results to the file - {0} ...", reportFileName));

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
                    reportText += "tr:nth-child(odd) {background-color: #eee;}";
                    reportText += "tr:nth-child(even) {background-color: #fff;}";
                    reportText += "th {background-color: black; color: white;";
                    reportText += "</style></head>";
                    reportText += "<body>";
                    reportText += "<h2>FileCompare - Global Search Report</h2>";
                    reportText += "<table table-layout: fixed; border-collapse: collapse; border: 3px solid black;><tr><th>File</th><th>Page Numbers</th></tr>";
                    reportText += GlobalSearchReportText;
                    reportText += "</body></html>";

                    sw.WriteLine(reportText);
                    GlobalSearchReportText = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void GenerateGlobalExceptionReport(string reportPath)
        {
            DateTime dateTime = DateTime.Now;
            string reportFileName = "pdf_global_exception_";
            reportFileName += dateTime.ToString("yyyyMMddHHmmss");
            reportFileName += ".html";
            string reportFilePath = Path.Combine(reportPath, reportFileName);
            Logger.Info(string.Format("Search::GenerateGlobalExceptionReport - Writing global exception results to the file - {0} ...", reportFileName));

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
                    reportText += "tr:nth-child(odd) {background-color: #eee;}";
                    reportText += "tr:nth-child(even) {background-color: #fff;}";
                    reportText += "th {background-color: black; color: white;";
                    reportText += "</style></head>";
                    reportText += "<body>";
                    reportText += "<h2>FileCompare - Global Search Exception Report</h2>";
                    reportText += "<table table-layout: fixed; border-collapse: collapse; border: 3px solid black;><tr><th>File</th><th>Page Numbers</th></tr>";
                    reportText += GlobalExceptionReportText;
                    reportText += "</body></html>";

                    sw.WriteLine(reportText);
                    GlobalExceptionReportText = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void GlobalSearchReportTextBuilder(List<int> searchPageList)
        {
            string reportText = "<tr>";
            reportText += "<td style=\"text-align: center\">";
            reportText += Path.GetFileNameWithoutExtension(FilePath);
            reportText += "</td>";
            reportText += "<td style=\"text-align: center\">";
            reportText += string.Join(",", searchPageList.Distinct());
            reportText += "</td>";
            reportText += "</tr>";

            GlobalSearchReportText += reportText;
        }

        public void GlobalExceptionReportTextBuilder(List<int> exceptionPageList)
        {
            string reportText = "<tr>";
            reportText += "<td style=\"text-align: center\">";
            reportText += Path.GetFileNameWithoutExtension(FilePath);
            reportText += "</td>";
            reportText += "<td style=\"text-align: center\">";
            reportText += string.Join(",", exceptionPageList.Distinct());
            reportText += "</td>";
            reportText += "</tr>";

            GlobalExceptionReportText += reportText;
        }

        /* ----------------------------------------------------------------
         * Endt:
         * Global search and exception report generation methods.
         * 
         * ------------------------------------------------------------- */

        /* ----------------------------------------------------------------
         * Start:
         * Individual search and exception report generation methods.
         * 
         * ------------------------------------------------------------- */

        public void GenerateSearchReports()
        {
            string reportFileName = Path.GetFileNameWithoutExtension(FilePath);
            reportFileName += "_search";
            reportFileName += ".html";
            string reportFilePath = Path.Combine(ReportPath, reportFileName);
            Logger.Info(string.Format("Search::GenerateSearchReports - Writing search results to the file - {0} ...", reportFileName));

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
                    reportText += "tr:nth-child(odd) {backgrouund-color: #eee;}";
                    reportText += "tr:nth-child(even) {background-color: #fff;}";
                    reportText += "th {background-color: black; color: white;";
                    reportText += "</style></head>";
                    reportText += "<body>";
                    reportText += "<h2>FileCompare - Search Report</h2>";
                    reportText += "<table table-layout: fixed; border-collapse: collapse; border: 3px solid purple;><tr><th>Page#</th><th>Pattern</th><th>Frequency</th></tr>";
                    reportText += SearchReportText;
                    reportText += "</table></body></html>";

                    sw.WriteLine(reportText);
                    SearchReportText = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void GenerateExceptionReports()
        {
            string reportFileName = Path.GetFileNameWithoutExtension(FilePath);
            reportFileName += "_exception.html";            
            string reportFilePath = Path.Combine(ReportPath, reportFileName);
            Logger.Info(string.Format("Search::GenerateExceptionReports - Writing exception results to the file - {0} ...", reportFileName));

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
                    reportText += "tr:nth-child(odd) {backgrouund-color: #eee;}";
                    reportText += "tr:nth-child(even) {background-color: #fff;}";
                    reportText += "th {background-color: black; color: white;";
                    reportText += "</style></head>";
                    reportText += "<body>";
                    reportText += "<h2>FileCompare - Search Exception Report</h2>";
                    reportText += "<table table-layout: fixed; border-collapse: collapse; border: 3px solid purple;><tr><th>Page#</th><th>Pattern</th><th>Frequency</th></tr>";
                    reportText += ExceptionReportText;
                    reportText += "</table></body></html>";

                    sw.WriteLine(reportText);
                    ExceptionReportText = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void SearchReportTextBuilder(Dictionary<string, int> dSearchPatterns, int pageNumber)
        {
            string reportText = "";

            List<string> kPatterns = dSearchPatterns.Keys.ToList();
            foreach (string pattern in kPatterns)
            {
                reportText += "<tr>";
                reportText += "<td style=\"text-align: center\">";
                reportText += pageNumber.ToString();
                reportText += "</td>";
                reportText += "<td style=\"text-align: center\">";
                reportText += pattern;
                reportText += "</td>";
                reportText += "<td style=\"text-align: center\">";
                reportText += dSearchPatterns[pattern].ToString();
                reportText += "</td>";
                reportText += "</tr>";
            }

            SearchReportText += reportText;
        }

        public void ExceptionReportTextBuilder(Dictionary<string, int> dExceptions, int pageNumber)
        {
            string reportText = "";

            List<string> kPatterns = dExceptions.Keys.ToList();
            foreach (string pattern in kPatterns)
            {
                reportText += "<tr>";
                reportText += "<td style=\"text-align: center\">";
                reportText += pageNumber.ToString();
                reportText += "</td>";
                reportText += "<td style=\"text-align: center\">";
                reportText += pattern;
                reportText += "</td>";
                reportText += "<td style=\"text-align: center\">";
                reportText += dExceptions[pattern].ToString();
                reportText += "</td>";
                reportText += "</tr>";
            }

            ExceptionReportText += reportText;
        }

        /* ----------------------------------------------------------------
         * End:
         * Global search and exception report generation methods.
         * 
         * ------------------------------------------------------------- */
    }
}
