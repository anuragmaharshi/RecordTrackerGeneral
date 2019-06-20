using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker
{
    public static class Constants
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();
        public static string GetDbFilePath()
        {

            //_logger.Info("DB file path set = " + ConfigurationManager.AppSettings["DbFilePath"]);
            // return ConfigurationManager.AppSettings["DbFilePath"];
            //var fileName = Path.Combine(Environment.GetFolderPath(
            //Environment.SpecialFolder.LocalApplicationData), "RecordTracker\\MainApplication.db");
            return "Resources/MainApplication.db";
        }
    }
}
