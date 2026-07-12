using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesss_Logic_Layer
{
    public static class clsLogger
    {
        private static string _logPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DVLD_Log.txt");

        public static void Log(string message, string level = "ERROR")
        {
            try
            {
                string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                File.AppendAllText(_logPath, entry + Environment.NewLine);
                System.Diagnostics.Debug.WriteLine(entry);
            }
            catch { }
        }

        public static void LogError(string message) => Log(message, "ERROR");
        public static void LogInfo(string message) => Log(message, "INFO");
    }
}
