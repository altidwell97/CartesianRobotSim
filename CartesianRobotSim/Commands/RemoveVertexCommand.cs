using System;
using System.Threading.Tasks;
using CartesianRobotSim.ViewModel;

namespace CartesianRobotSim.Commands
{
    // Removes the selected vertex from a PositionListEntryViewModel's AddedPositions collection.
    public class RemoveVertexCommand : AsyncCommandBase
    {
        private PositionListEntryViewModel? _attachedVm;

        /// <summary>
        /// Attaches to a PositionListEntryViewModel to monitor its state and enable/disable the command accordingly.
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
        /// When the attached ViewModel's properties change, this method checks if the SelectedPoint property has changed 
        /// and raises the CanExecuteChanged event to update the command's enabled state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVmPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionListEntryViewModel.SelectedPoint))
            {
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// When the attached ViewModel's AddedPositions collection changes, this method raises the CanExecuteChanged event to update the command's enabled state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            var vm = _attachedVm ?? parameter as PositionListEntryViewModel;
            if (vm == null) return false;
            return vm.SelectedPoint != null && vm.AddedPositions.Count > 0 && base.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command asynchronously, removing the selected vertex from the attached ViewModel's AddedPositions collection.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override Task ExecuteAsync(object? parameter)
        {
            var vm = parameter as PositionListEntryViewModel ?? _attachedVm;
            if (vm == null) return Task.CompletedTask;

            var selected = vm.SelectedPoint;
            if (selected != null)
            {
                vm.AddedPositions.Remove(selected);
                vm.SelectedPoint = null;
            }

            return Task.CompletedTask;
        }
    }
}
