using CartesianRobotSim.Model;
using CartesianRobotSim.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CartesianRobotSim.Commands
{
    public class LoadMemorizedPathsCommand : AsyncCommandBase
    {
        private readonly MemorizedPaths _memorizedPaths;
        private readonly MemorizedPositionsViewModel _memorizedPositionsViewModel;

        public LoadMemorizedPathsCommand(MemorizedPaths memorizedPaths, MemorizedPositionsViewModel memorizedPositionsViewModel)
        {
            _memorizedPaths = memorizedPaths;
            _memorizedPositionsViewModel = memorizedPositionsViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try 
            {
                IEnumerable<Path> paths = await _memorizedPaths.GetAllPaths();

                _memorizedPositionsViewModel.LoadPaths(paths);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading memorized paths: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
