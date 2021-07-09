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
        public PdfSearch(String filePath, List<string> searchTexts, List<string> exceptions, string reportPath)
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
                Logger.Error("PdfSearch::SearchTextInFile - File does not exist. Aborting search!");
                return;
            }

            PdfDocument document = new PdfDocument(new PdfReader(FilePath));
            int totalPageCount = document.GetNumberOfPages();
            var pageText = new System.Text.StringBuilder();

            List<int> searchPageList = new List<int>();
            List<int> exceptionPageList = new List<int>();
          
            for (int i = 1; i <= totalPageCount; i++)
            {
                ITextExtractionStrategy textExtractionStrategy = new SimpleTextExtractionStrategy();
                PdfPage page = document.GetPage(i);
                pageText.AppendLine(PdfTextExtractor.GetTextFromPage(page, textExtractionStrategy));

                // Search for patterns in this page.
                if (SearchTexts.Count > 0)
                {
                    Dictionary<string, int> dSearchPatterns = new Dictionary<string, int>();
                    int searchCount = 0;
                    Console.WriteLine("PageText = " + pageText);
                    foreach (string searchText in SearchTexts)
                    {
                        searchCount = Regex.Matches(pageText.ToString(), searchText).Count;
                        dSearchPatterns.Add(searchText, searchCount);

                        // If we have a match from the search set then add the page number.
                        if (searchCount > 0)
                        {
                            searchPageList.Add(i);
                        }
                    }

                    // Build the individual report text for this page.
                    SearchReportTextBuilder(dSearchPatterns, i);
                }

                // Search for exception patterns in this page.
                if (Exceptions.Count > 0)
                {
                    Dictionary<string, int> dExceptions = new Dictionary<string, int>();
                    int excCount = 0;

                    foreach (string exception in Exceptions)
                    {
                        excCount = Regex.Matches(pageText.ToString(), exception).Count;
                        dExceptions.Add(exception, excCount);

                        // If we have a match from the search set then add the page number.
                        if (excCount > 0)
                        {
                            exceptionPageList.Add(i);
                        }
                    }

                    // Build the individual report text for this page.
                    ExceptionReportTextBuilder(dExceptions, i);
                }
            }

            // Generate the individual reports for this file.
            GenerateSearchReports();
            GenerateExceptionReports();

            // Build the global report texts for this file.
            GlobalSearchReportTextBuilder(searchPageList);
            GlobalExceptionReportTextBuilder(exceptionPageList);
        }
    }
}
