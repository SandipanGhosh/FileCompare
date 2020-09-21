using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace FileCompare
{
    class QueryCompareDirs
    {
        public string Directory1 { get; set; }
        public string Directory2 { get; set; }

        public QueryCompareDirs(string directory1, string directory2)
        {
            Directory1 = directory1;
            Directory2 = directory2;
        }

        public void CompareDirectories()
        {
            DirectoryInfo dir1 = new DirectoryInfo(Directory1);
            DirectoryInfo dir2 = new DirectoryInfo(Directory2);

            // Take a snapshot of the file system.
            IEnumerable<FileInfo> list1 = dir1.GetFiles("*.*", SearchOption.AllDirectories);
            IEnumerable<FileInfo> list2 = dir2.GetFiles("*.*", SearchOption.AllDirectories);

            // A custom file comparer defined below
            DirectoryCompare myFileCompare = new DirectoryCompare();

            // This query determines whether the two folders contain identical file lists,
            // based on the custom file comparer that is defined in the FileComparer class.
            // The query executes immediately because it returns a bool.
            bool areIdentical = list1.SequenceEqual(list2, myFileCompare);

            if (areIdentical == true)
            {
                Logger.Info("QueryCompareDirs::CompareDirectories - The two directories are identical.");
            }
            else
            {
                Logger.Info("QueryCompareDirs::CompareDirectories - The two directories are not identical.");
            }

            // Find the common files. It produces a sequence and doesn't execute until the
            // foreach statement.
            var queryCommonFiles = list1.Intersect(list2, myFileCompare);
            
            if (queryCommonFiles.Any())
            {
                Logger.Info("QueryCompareDirs::CompareDirectories - The following files are in both directories:");
                foreach(var v in queryCommonFiles)
                {
                    // Shows which items end up in result list.
                    Logger.Info(string.Format("QueryCompareDirs::CompareDirectories - {0}", v.FullName));
                }
            }
            else
            {
                Logger.Info("QueryCompareDirs::CompareDirectories - There are no common files in the two directories.");
            }

            // Find the set difference between the two directories.
            // Here we only check one way.
            var queryList1Only = (from file in list1
                                  select file).Except(list2, myFileCompare);
            Logger.Info("QueryCompareDirs::CompareDirectories - The following files are in list1 but not list2:");
            foreach(var v in queryList1Only)
            {
                Logger.Info(string.Format("QueryCompareDirs::CompareDirectories - {0}", v.FullName));
            }

            var queryList2Only = (from file in list2
                                  select file).Except(list1, myFileCompare);
            Logger.Info("QueryCompareDirs::CompareDirectories - The following files are in list2 but not list1:");
            foreach (var v in queryList2Only)
            {
                Logger.Info(string.Format("QueryCompareDirs::CompareDirectories - {0}", v.FullName));
            }         
        }
    }
}
