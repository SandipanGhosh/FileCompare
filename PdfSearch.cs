using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace FileCompare
{
    public class PdfSearch : Search
    {
        public PdfSearch(String filePath, List<string> searchTexts, string reportPath)
        {
            FilePath = filePath;
            SearchTexts = searchTexts;
            ReportPath = reportPath;
        }

        public override void SearchTextInFile()
        {
            if (!File.Exists(FilePath))
            {
                Logger.Error("PdfSearch::SearchTextInFile - File does not exist. Aborting search!");
                return;
            }

            PdfDocument document = new PdfDocument(new PdfReader(FilePath));
            int totalPageCount = document.GetNumberOfPages();
            var currentText = new System.Text.StringBuilder();
            for (int i = 1; i <= totalPageCount; i++)
            {
                ITextExtractionStrategy textExtractionStrategy = new SimpleTextExtractionStrategy();
                PdfPage page = document.GetPage(i);
                currentText.AppendLine(PdfTextExtractor.GetTextFromPage(page, textExtractionStrategy));
            }

            Dictionary<string, int> dSearchPatterns = new Dictionary<string, int>();
            foreach (string searchText in SearchTexts)
            {
                dSearchPatterns.Add(searchText, Regex.Matches(currentText.ToString(), searchText).Count);
            }

            GenerateSearchReport(dSearchPatterns);
        }
    }
}
