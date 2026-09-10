using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CartesianRobotSim.Commands
{
    public class MakeVertexCommand : AsyncCommandBase
    {
        private readonly List<Vertex> _path;
        private readonly double _xValue;
        private readonly double _yValue;
        private readonly double _zValue;

        public MakeVertexCommand(double x, double y, double z, List<Vertex> path)
        {
            _xValue = x;
            _yValue = y;
            _zValue = z;
            _path = path;
        }

        public override bool CanExecute(object? parameter)
        {
            int length = _path.Count();
            if (length < 5)
            {
                return true;
            }

            // If an attached VM is provided, set an inline message instead of showing a MessageBox
            if (parameter is CartesianRobotSim.ViewModel.PositionListEntryViewModel vm)
            {
                vm.AddMessage = "Path is full. Cannot add more vertices.";
            }

            return false;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            Vertex vertex = new Vertex(_xValue, _yValue, _zValue);

            try
            {
                _path.Add(vertex);

                if (parameter is CartesianRobotSim.ViewModel.PositionListEntryViewModel vm)
                {
                    vm.AddMessage = "Vertex added.";
                    // Clear message after a short delay
                    _ = System.Threading.Tasks.Task.Run(async () =>
                    {
                        await System.Threading.Tasks.Task.Delay(1200).ConfigureAwait(false);
                        var disp = System.Windows.Application.Current?.Dispatcher;
                        if (disp == null || disp.CheckAccess()) vm.AddMessage = null;
                        else disp.Invoke(() => vm.AddMessage = null);
                    });
                }
            }
            catch (Exception ex)
            {
                try { CartesianRobotSim.Services.Logging.StartupLogger.LogException(ex); } catch { }
                if (parameter is CartesianRobotSim.ViewModel.PositionListEntryViewModel vm)
                {
                    vm.AddMessage = "Failed to add vertex.";
                }
            }
        }
    }
}
