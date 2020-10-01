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
        public DocSearch(String filePath, List<string> searchTexts, string reportPath)
        {
            FilePath = filePath;
            SearchTexts = searchTexts;
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

                Dictionary<string, int> dSearchPatterns = new Dictionary<string, int>();
                foreach (string searchText in SearchTexts)
                {
                    dSearchPatterns.Add(searchText, Regex.Matches(body.InnerText, searchText).Count);
                }

                GenerateSearchReport(dSearchPatterns);
            }
        }
    }
}
