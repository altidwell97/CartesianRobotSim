using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.ViewModel
{
    public class RobotEnvironmentViewModel : ViewModelBase
    {
        private double _pointerX;
        public double PointerX
        {
            get 
            { 
                return _pointerX; 
            }
            set
            {
                _pointerX = value;
                OnPropertyChanged(nameof(PointerX));
            }
        }

        private double _pointerY;
        public double PointerY
        {
            get
            {
                return _pointerY;
            }
            set
            {
                _pointerY = value;
                OnPropertyChanged(nameof(PointerY));
            }
        }

        private double _pointerZ;
        public double PointerZ
        {
            get
            {
                return _pointerZ;
            }
            set
            {
                _pointerZ = value;
                OnPropertyChanged(nameof(PointerZ));
            }
        }

        public RobotEnvironmentViewModel()
        {

        }
    }
}
