using System;
using System.Threading.Tasks;
using CartesianRobotSim.ViewModel;

namespace CartesianRobotSim.Commands
{
    // Removes the selected vertex from a PositionListEntryViewModel's AddedPositions collection.
    public class RemoveVertexCommand : AsyncCommandBase
    {
        private PositionListEntryViewModel? _attachedVm;

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

        private void OnVmPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionListEntryViewModel.SelectedPoint))
            {
                OnCanExecuteChanged();
            }
        }

        private void OnCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            var vm = _attachedVm ?? parameter as PositionListEntryViewModel;
            if (vm == null) return false;
            return vm.SelectedPoint != null && vm.AddedPositions.Count > 0 && base.CanExecute(parameter);
        }

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
