using System;
using System.IO;

namespace FileCompare
{
    public class FileCompare
    {
        static int Main(string[] args)
        {
            // Read the config params from the json file
            var vJsonString = File.ReadAllText("FileCompare.json");
            if (!(vJsonString.Length > 0))
            {
                //Logger.Error("Read from json failed!");
                return -1;
            }

            return 0;
        }
    }
}
