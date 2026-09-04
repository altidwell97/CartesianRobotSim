using CartesianRobotSim.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CartesianRobotSim.ViewModel
{
    public class CommandControlsViewModel : ViewModelBase
    {
        private double _xDistance;
        public double XDistance
        {
            get { return _xDistance; }
            set
            {
                _xDistance = value;
                OnPropertyChanged(nameof(XDistance));
            }
        }

        private double _yDistance;
        public double YDistance
        {
            get { return _yDistance; }
            set
            {
                _yDistance = value;
                OnPropertyChanged(nameof(YDistance));
            }
        }

        private double _zDistance;
        public double ZDistance
        {
            get { return _zDistance; }
            set
            {
                _zDistance = value;
                OnPropertyChanged(nameof(ZDistance));
            }
        }

        public ICommand MoveCommand { get; }

        public CommandControlsViewModel()
        {

        }

    }
}
