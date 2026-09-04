using CartesianRobotSim.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace CartesianRobotSim.ViewModel
{
    public class MemorizedPositionsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<VertexViewModel> _memorizedPositions;

        public IEnumerable<VertexViewModel> MemorizedPositions => _memorizedPositions;
        public ICommand RunCommand { get; }

        public MemorizedPositionsViewModel()
        {
            _memorizedPositions = new ObservableCollection<VertexViewModel>();
        }

    }
}
