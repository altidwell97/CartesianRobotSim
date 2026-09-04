using CartesianRobotSim.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CartesianRobotSim.ViewModel
{
    public class PositionListEntryViewModel :ViewModelBase
    {
        private double _xValue;
        public double XValue
        {
            get 
            {
                return _xValue; 
            }
            set
            {
                _xValue = value;
                OnPropertyChanged(nameof(XValue));
            }
        }

        private double _yValue;
        public double YValue
        {
            get
            {
                return _yValue;
            }
            set
            {
                _yValue = value;
                OnPropertyChanged(nameof(YValue));
            }
        }

        private double _zValue;
        public double ZValue
        {
            get
            {
                return _zValue;
            }
            set
            {
                _zValue = value;
                OnPropertyChanged(nameof(ZValue));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand SaveCommand { get; }

        public PositionListEntryViewModel()
        {

        }
    }
}
