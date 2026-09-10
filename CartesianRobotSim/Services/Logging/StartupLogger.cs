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

        /// <summary>
        /// Formats and writes a log message to the startup log file and debug output.
        /// </summary>
        /// <param name="message"></param>
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

        /// <summary>
        /// Formats and writes an exception message to the startup log file and debug output.
        /// </summary>
        /// <param name="ex"></param>
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
