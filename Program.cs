using System;
using System.Windows;

namespace CartesianRobotSim
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception ex)
            {
                // Last-resort logging for startup failures
                try
                {
                    var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AltIdwell97", "CartesianRobotSim");
                    System.IO.Directory.CreateDirectory(folder);
                    var log = System.IO.Path.Combine(folder, "startup_log.txt");
                    System.IO.File.AppendAllText(log, $"[Main Exception] {ex}\n");
                }
                catch { }
                try { MessageBox.Show("Fatal startup error:\n" + ex.ToString(), "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error); } catch { }
            }
        }
    }
}
