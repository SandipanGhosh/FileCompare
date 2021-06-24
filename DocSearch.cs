using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;

namespace FileCompare
{
    public class DocSearch : Search
    {
        public DocSearch(String filePath, List<string> searchTexts, List<string> exceptions, string reportPath)
        {
            FilePath = filePath;
            SearchTexts = searchTexts;
            Exceptions = exceptions;
            ReportPath = reportPath;
        }

        public override void SearchTextInFile()
        {
            if (!File.Exists(FilePath))
            {
                Logger.Error("DocSearch::SearchTextInFile - File does not exist. Aborting search!");
                return;
            }

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(FilePath, false))
            {
                DocumentFormat.OpenXml.Wordprocessing.Body body = wordDoc.MainDocumentPart.Document.Body;

                // Search for patterns and generate report.
                if (SearchTexts.Count > 0)
                {
                    Dictionary<string, int> dSearchPatterns = new Dictionary<string, int>();
                    foreach (string searchText in SearchTexts)
                    {
                        dSearchPatterns.Add(searchText, Regex.Matches(body.InnerText, searchText).Count);
                    }

                    //GenerateSearchReport(dSearchPatterns, 1);
                }

                // Search for exceptions and generate report if they occur.
                if (Exceptions.Count > 0)
                {
                    Dictionary<string, int> dExceptions = new Dictionary<string, int>();
                    int count = 0;
                    bool frequency = false;
                    foreach (string exception in Exceptions)
                    {
                        count = Regex.Matches(body.InnerText, exception).Count;
                        dExceptions.Add(exception, count);
                        if (count > 0)
                        {
                            frequency = true;
                        }
                    }

                    // Check if atleast one from the exception set occurs in the file. If true then
                    // generate exception report.
                    if (true == frequency)
                    {
                        //GenerateExceptionReport(dExceptions);
                        //SearchExceptionReportBuilder(dExceptions);
                    }
                }
            }
        }
    }
}
