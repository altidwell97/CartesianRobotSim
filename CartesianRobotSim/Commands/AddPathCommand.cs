using CartesianRobotSim.Stores;
using CartesianRobotSim.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using CartesianRobotSim.Model;

namespace CartesianRobotSim.Commands
{
    public class AddPathCommand : AsyncCommandBase
    {
        private readonly MemorizedPathsStore _memorizedPathsStore;
        private ViewModel.PositionListEntryViewModel? _attachedVm;

        public AddPathCommand(MemorizedPathsStore memorizedPathsStore)
        {
            _memorizedPathsStore = memorizedPathsStore;
        }


        public override bool CanExecute(object? parameter)
        {
            // Prefer attached VM; fall back to parameter if provided
            var vm = _attachedVm ?? parameter as ViewModel.PositionListEntryViewModel;
            if (vm == null) return false;

            return vm.AddedPositions != null && vm.AddedPositions.Count > 0;
        }

        /// <summary>
        /// Adds the current list of positions from the PositionListEntryViewModel to the MemorizedPathsStore as a new Path.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override async Task ExecuteAsync(object? parameter)
        {
            // Prefer attached VM; fall back to parameter if provided
            var vm = parameter as PositionListEntryViewModel ?? _attachedVm;
            if (vm == null) return;

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

        /// <summary>
        /// Handles the PropertyChanged event of the attached ViewModel, updating the command's enabled state when the Position property changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Position changes can affect whether saving is allowed
            if (e.PropertyName == nameof(PositionListEntryViewModel.Position))
            {
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the attached ViewModel's AddedPositions collection, 
        /// updating the command's enabled state when the collection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// Attaches the command to a specific PositionListEntryViewModel, subscribing to its PropertyChanged 
        /// and CollectionChanged events to update the command's enabled state.
        /// </summary>
        /// <param name="vm"></param>
        public void Attach(ViewModel.PositionListEntryViewModel? vm)
        {
            if (_attachedVm != null)
            {
                _attachedVm.PropertyChanged -= OnViewModelPropertyChanged;
                _attachedVm.AddedPositions.CollectionChanged -= OnCollectionChanged;
            }

            _attachedVm = vm;

            if (_attachedVm != null)
            {
                _attachedVm.PropertyChanged += OnViewModelPropertyChanged;
                _attachedVm.AddedPositions.CollectionChanged += OnCollectionChanged;
            }

            OnCanExecuteChanged();
        }
    }
}
