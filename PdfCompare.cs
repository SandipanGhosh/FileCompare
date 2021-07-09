
using iText.Kernel;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace FileCompare
{
    public class PdfCompare : Compare
    {
        public PdfCompare(List<Dictionary<string, string>> pdfCompareSet, string rootPath, string reportPath)
        {
            CompareSet = pdfCompareSet;
            RootPath = rootPath;
            ReportPath = reportPath;
        }

        public override void CompareFiles()
        {
            Logger.Info("PdfCompare::CompareFiles - Starting pdf compare ...");

            foreach(Dictionary<string, string> dictFiles in CompareSet)
            {
                FilePath1 = Path.Combine(RootPath, dictFiles["file1"].ToString());
                FilePath2 = Path.Combine(RootPath, dictFiles["file2"].ToString());

                Logger.Info(string.Format("PdfCompare::CompareFiles - File1:{0}, File2:{1}", FilePath1, FilePath2));
                if (!File.Exists(FilePath1) || !File.Exists(FilePath2))
                {
                    Logger.Error("PdfCompare::CompareFiles - Either of the file in the set does not exist. Aborting compare for the set!");
                    continue;
                }

                ComparePdfFilesText();
                ComparePdfFilesImages();
            }
        }

        private void ComparePdfFilesText()
        {
            CompareFilesForDiffPatch(ExtractTextFromPdf(FilePath1), ExtractTextFromPdf(FilePath2));
        }

        private void ComparePdfFilesImages()
        {
            String imageBitmap1 = ManipulatePdfImages(FilePath1);
            String imageBitmap2 = ManipulatePdfImages(FilePath2);
            if (imageBitmap1.Equals(imageBitmap2))
                Logger.Info("PdfCompare::ComparePdfFileImages - No image differences found.");
            else
                Logger.Info("PdfCompare::ComparePdfFileImages - Differences in images found.");
        }

        private string ExtractTextFromPdf(String src)
        {
            // Extract text from the pdf file
            PdfDocument document = new PdfDocument(new PdfReader(src));
            int totalPageCount = document.GetNumberOfPages();
            var currentText = new System.Text.StringBuilder();
            for (int i = 1; i <= totalPageCount; i++)
            {
                ITextExtractionStrategy textExtractionStrategy = new SimpleTextExtractionStrategy();
                PdfPage page = document.GetPage(i);
                currentText.AppendLine(PdfTextExtractor.GetTextFromPage(page, textExtractionStrategy));
            }

            return currentText.ToString();
        }

        private String ManipulatePdfImages(string srcFilePath)
        {
            Logger.Info(string.Format("PdfCompare::ManipulatePdfImages - Processing images for {0} ...", srcFilePath));
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(srcFilePath));
            String imageBitmap = "";

            int numberOfPdfObject = pdfDoc.GetNumberOfPdfObjects();
            for (int i = 1; i <= numberOfPdfObject; i++)
            {
                PdfObject obj = pdfDoc.GetPdfObject(i);

                if (obj != null && obj.IsStream())
                {
                    byte[] b;
                    try
                    {
                        // Get decoded stream bytes.
                        b = ((PdfStream)obj).GetBytes();
                    }
                    catch(PdfException)
                    {
                        // Get originally encoded stream bytes.
                        b = ((PdfStream)obj).GetBytes(false);
                    }

                    MemoryStream mos = new MemoryStream();
                    mos.Write(b, 0, b.Length);
                    imageBitmap = Convert.ToBase64String(mos.ToArray());
                }
            }

            pdfDoc.Close();
            return imageBitmap;
        }
    }
}
