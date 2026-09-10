using CartesianRobotSim.ViewModel;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using CartesianRobotSim.Model;

namespace CartesianRobotSim.Commands
{
    // Adds a vertex to the PositionListEntryViewModel's AddedPositions collection.
    public class AddVertexCommand : CommandBase
    {
        private PositionListEntryViewModel? _attachedVm;
        private const double _epsilon = 1e-9;

        /// <summary>
        /// Attaches the command to a specific PositionListEntryViewModel instance, allowing it to listen for property changes and collection changes.
        /// </summary>
        /// <param name="vm"></param>
        public void Attach(PositionListEntryViewModel vm)
        {
            if (_attachedVm != null)
            {
                _attachedVm.PropertyChanged -= OnVmPropertyChanged;
                _attachedVm.AddedPositions.CollectionChanged -= OnCollectionChanged;
            }

            _attachedVm = vm;

            if (_attachedVm != null)
            {
                _attachedVm.PropertyChanged += OnVmPropertyChanged;
                _attachedVm.AddedPositions.CollectionChanged += OnCollectionChanged;
            }

            OnCanExecuteChanged();
        }

        /// <summary>
        /// When the attached ViewModel's properties change, this method checks if the X, Y, or Z values have changed
        /// and raises the CanExecuteChanged event to update the command's enabled state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionListEntryViewModel.XValue) || e.PropertyName == nameof(PositionListEntryViewModel.YValue) || e.PropertyName == nameof(PositionListEntryViewModel.ZValue))
            {
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// When the attached ViewModel's AddedPositions collection changes, this method raises the CanExecuteChanged event to update the command's enabled state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// Determines whether the command can execute based on the state of the attached ViewModel or the provided parameter.

        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object? parameter)
        {
            // Prefer attached VM; fall back to parameter if provided
            var vm = _attachedVm ?? parameter as PositionListEntryViewModel;
            if (vm == null) return false;

            // Check maximum count
            if (vm.AddedPositions.Count >= 5)
            {
                vm.AddDisabledMessage = "Path already has 5 vertices.";
                return false;
            }

            // Only disallow if the current input matches the most recently added vertex
            if (vm.AddedPositions.Count > 0)
            {
                var last = vm.AddedPositions[vm.AddedPositions.Count - 1];
                if (Math.Abs(last.XValue - vm.XValue) <= _epsilon && Math.Abs(last.YValue - vm.YValue) <= _epsilon && Math.Abs(last.ZValue - vm.ZValue) <= _epsilon)
                {
                    vm.AddDisabledMessage = "Cannot add duplicate of the last vertex.";
                    return false;
                }
            }

            // If we reach here, enable and clear any message
            vm.AddDisabledMessage = null;
            return base.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command, adding a new vertex to the attached ViewModel's AddedPositions collection.
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object? parameter)
        {
            var vm = parameter as PositionListEntryViewModel ?? _attachedVm;
            if (vm == null) return;

            var v = new Vertex(vm.XValue, vm.YValue, vm.ZValue);
            vm.AddedPositions.Add(v);
            vm.Position = v; // update Position property for other listeners

            // Show inline add confirmation
            try
            {
                var disp = System.Windows.Application.Current?.Dispatcher;
                void SetMsg() => vm.AddMessage = "Vertex added.";

                if (disp == null || disp.CheckAccess())
                    SetMsg();
                else
                    disp.Invoke(SetMsg);

                // Clear after a short delay
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(1200).ConfigureAwait(false);
                    var ui = System.Windows.Application.Current?.Dispatcher;
                    if (ui == null || ui.CheckAccess()) vm.AddMessage = null;
                    else ui.Invoke(() => vm.AddMessage = null);
                });
            }
            catch { }
        }
    }
}
