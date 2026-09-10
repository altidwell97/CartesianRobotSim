using System;
using System.IO;
using System.Text;

namespace CartesianRobotSim.Services.Logging
{
    internal static class StartupLogger
    {
        private static readonly string LogFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CartesianRobotSim");
        private static readonly string LogFile = System.IO.Path.Combine(LogFolder, "startup_log.txt");

        static StartupLogger()
        {
            try { Directory.CreateDirectory(LogFolder); } catch { }
        }

        public static void Log(string message)
        {
            try
            {
                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}{Environment.NewLine}";
                File.AppendAllText(LogFile, line, Encoding.UTF8);
                System.Diagnostics.Debug.WriteLine(message);
            }
            catch { }
        }

        public static void LogException(Exception ex)
        {
            try
            {
                var text = ex?.ToString() ?? "(null)";
                Log("EXCEPTION: " + text);
            }
            catch { }
        }
    }
}
