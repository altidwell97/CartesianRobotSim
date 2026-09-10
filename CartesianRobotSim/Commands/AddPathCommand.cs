using CartesianRobotSim.Stores;
using CartesianRobotSim.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using CartesianRobotSim.Model;

namespace CartesianRobotSim.Commands
{
    public class AddPathCommand : AsyncCommandBase
    {
        private readonly MemorizedPathsStore _memorizedPathsStore;

        public AddPathCommand(MemorizedPathsStore memorizedPathsStore)
        {
            _memorizedPathsStore = memorizedPathsStore;
        }

        // Cannot execute if the path already has 5 positions
        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            // Expect the PositionListEntryViewModel to be passed as the command parameter
            if (!(parameter is PositionListEntryViewModel vm)) return;

            var added = vm.AddedPositions;
            if (added == null || added.Count == 0) return;

            var vertices = new List<Vertex>(added as IEnumerable<Vertex>);
            var path = new Path(vertices);

            try
            {
                await _memorizedPathsStore.AddPath(path);

                // Clear the VM positions on the UI thread and reset entry state
                var dispatcher = System.Windows.Application.Current?.Dispatcher;
                void ClearUiState()
                {
                    vm.AddedPositions.Clear();
                    vm.Position = null;
                    vm.SelectedPoint = null;
                    vm.AddDisabledMessage = null;
                }

                if (dispatcher == null || dispatcher.CheckAccess())
                {
                    ClearUiState();
                    // Show save message briefly
                    vm.SaveMessage = "Path saved.";
                }
                else
                {
                    dispatcher.Invoke(() =>
                    {
                        ClearUiState();
                        vm.SaveMessage = "Path saved.";
                    });
                }

                // Clear save message after a short delay without blocking the UI
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        await System.Threading.Tasks.Task.Delay(1500).ConfigureAwait(false);
                        var uiDisp = System.Windows.Application.Current?.Dispatcher;
                        if (uiDisp == null || uiDisp.CheckAccess())
                        {
                            vm.SaveMessage = null;
                        }
                        else
                        {
                            uiDisp.Invoke(() => vm.SaveMessage = null);
                        }
                    }
                    catch { }
                });
            }
            catch (Exception ex)
            {
                // Log and swallow to avoid triggering global exception handlers/UI message boxes
                try { CartesianRobotSim.Services.Logging.StartupLogger.LogException(ex); } catch { }
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionListEntryViewModel.Position))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
