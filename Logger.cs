using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using log4net.Repository;

namespace FileCompare
{
    public static class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void InitializeLogger(string logFilePath)
        {
            ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
            var fileInfo = new FileInfo(@"app.config");
            log4net.GlobalContext.Properties["LogFileName"] = logFilePath;
            log4net.Config.XmlConfigurator.Configure(repository, fileInfo);
        }

        public static void Info(string text)
        {
            log.Info(text);
        }

        public static void Error(string text)
        {
            log.Error(text);
        }

        public static void Debug(string text)
        {
            log.Debug(text);
        }
    }
}
