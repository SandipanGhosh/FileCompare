using DiffMatchPatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileCompare
{
    public abstract class Compare
    {
        public string FilePath1 { get; set; }
        public string FilePath2 { get; set; }
        public List<Dictionary<string, string>> CompareSet { get; set; }
        public string RootPath { get; set; }
        public string ReportPath { get; set; }

        public abstract void CompareFiles();

        public void CompareFilesForDiffPatch(string text1, string text2)
        {
            // Compute the diffs and save to a new html file only if they are not equal
            var diffs = Diff.Compute(text1, text2);
            diffs.CleanupSemantic();

            if (diffs.Count() == 1)
            {
                Logger.Info("Compare::CompareFilesForDiffPatch - The files are equal!");
                return;
            }

            string htmlDiffFileName = Path.GetFileNameWithoutExtension(FilePath1);
            htmlDiffFileName += "_";
            htmlDiffFileName += Path.GetFileNameWithoutExtension(FilePath2);
            htmlDiffFileName += "_diffs.html";
            string htmlDiffFile = Path.Combine(ReportPath, htmlDiffFileName);
            Logger.Info(string.Format("Compare::CompareFilesForDiffPatch - Writing diffs to the file - {0} ...", htmlDiffFileName));

            try
            {
                if (File.Exists(htmlDiffFile))
                {
                    File.Delete(htmlDiffFile);
                }

                using (StreamWriter sw = File.CreateText(htmlDiffFile))
                {
                    sw.WriteLine(diffs.ToHtml());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // Compute the patch and save to a new html file
            var patches = Patch.Compute(text1, text2);

            string htmlPatchFileName = Path.GetFileNameWithoutExtension(FilePath1);
            htmlPatchFileName += "_";
            htmlPatchFileName += Path.GetFileNameWithoutExtension(FilePath2);
            htmlPatchFileName += "_patch.html";
            string htmlPatchFile = Path.Combine(ReportPath, htmlPatchFileName);
            Logger.Info(string.Format("Compare::CompareFilesForDiffPatch - Writing patch to the file - {0} ...", htmlPatchFileName));

            try
            {
                if (File.Exists(htmlPatchFile))
                {
                    File.Delete(htmlPatchFile);
                }

                using (StreamWriter sw = File.CreateText(htmlPatchFile))
                {
                    sw.WriteLine(patches.ToHtml());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
