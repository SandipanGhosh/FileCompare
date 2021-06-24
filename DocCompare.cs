using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml.Packaging;

namespace FileCompare
{
    public class DocCompare : Compare
    {
        public DocCompare(List<Dictionary<string, string>> docCompareSet, string rootPath, string reportPath)
        {
            CompareSet = docCompareSet;
            RootPath = rootPath;
            ReportPath = reportPath;
        }

        public override void CompareFiles()
        {
            Logger.Info("DocCompare::CompareFiles - Starting doc compare ...");

            foreach (Dictionary<string, string> dictFiles in CompareSet)
            {
                FilePath1 = Path.Combine(RootPath, dictFiles["file1"].ToString());
                FilePath2 = Path.Combine(RootPath, dictFiles["file2"].ToString());

                Logger.Info(string.Format("DocCompare::CompareFiles - File1:{0}, File2:{1}", FilePath1, FilePath2));
                if (!File.Exists(FilePath1) || !File.Exists(FilePath2))
                {
                    Logger.Error("DocCompare::CompareFiles - Either of the file in the set does not exist. Aborting compare for the set!");
                    continue;
                }

                CompareDocFilesText();
            }
        }

        private void CompareDocFilesText()
        {
            CompareFilesForDiffPatch(ExtractTextFromDoc(FilePath1), ExtractTextFromDoc(FilePath2));
        }

        private string ExtractTextFromDoc(string srcFilePath)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(srcFilePath, false))
            {
                DocumentFormat.OpenXml.Wordprocessing.Body body = wordDoc.MainDocumentPart.Document.Body;
                return (body.InnerText);
            }
        }
    }
}
