using CartesianRobotSim.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CartesianRobotSim.ViewModel
{
    public class ManualControlViewModel : ViewModelBase
    {
        public ICommand PlusYCommand { get; }
        public ICommand MinusYCommand { get; }
        public ICommand PlusXCommand { get; }
        public ICommand MinusXCommand { get; }
        public ICommand PlusZCommand { get; }
        public ICommand MinusZCommand { get; }

        public ManualControlViewModel()
        {

        }
    }
}
