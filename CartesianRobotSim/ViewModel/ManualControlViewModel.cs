using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CartesianRobotSim.Commands;
using CartesianRobotSim.Model;

namespace CartesianRobotSim.ViewModel
{
    public class ManualControlViewModel : ViewModelBase
    {
        private double _xDistance = 1.0;
        public double XDistance
        {
            get => _xDistance;
            set { _xDistance = value; OnPropertyChanged(nameof(XDistance)); }
        }

        private double _yDistance = 1.0;
        public double YDistance
        {
            get => _yDistance;
            set { _yDistance = value; OnPropertyChanged(nameof(YDistance)); }
        }

        private double _zDistance = 1.0;
        public double ZDistance
        {
            get => _zDistance;
            set { _zDistance = value; OnPropertyChanged(nameof(ZDistance)); }
        }

        public ICommand PlusYCommand { get; }
        public ICommand MinusYCommand { get; }
        public ICommand PlusXCommand { get; }
        public ICommand MinusXCommand { get; }
        public ICommand PlusZCommand { get; }
        public ICommand MinusZCommand { get; }

        private readonly RobotEnvironmentViewModel _environment;
        private readonly MoveToVertexCommand _moveToVertexCommand;

        public ManualControlViewModel(MoveToVertexCommand moveToVertexCommand, RobotEnvironmentViewModel environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _moveToVertexCommand = moveToVertexCommand ?? throw new ArgumentNullException(nameof(moveToVertexCommand));

            PlusXCommand = new RelayCommand(_ => _moveToVertexCommand.Execute(new Vertex(_environment.PointerX + XDistance, _environment.PointerY, _environment.PointerZ)));
            MinusXCommand = new RelayCommand(_ => _moveToVertexCommand.Execute(new Vertex(_environment.PointerX - XDistance, _environment.PointerY, _environment.PointerZ)));
            PlusYCommand = new RelayCommand(_ => _moveToVertexCommand.Execute(new Vertex(_environment.PointerX, _environment.PointerY + YDistance, _environment.PointerZ)));
            MinusYCommand = new RelayCommand(_ => _moveToVertexCommand.Execute(new Vertex(_environment.PointerX, _environment.PointerY - YDistance, _environment.PointerZ)));
            PlusZCommand = new RelayCommand(_ => _moveToVertexCommand.Execute(new Vertex(_environment.PointerX, _environment.PointerY, _environment.PointerZ + ZDistance)));
            MinusZCommand = new RelayCommand(_ => _moveToVertexCommand.Execute(new Vertex(_environment.PointerX, _environment.PointerY, _environment.PointerZ - ZDistance)));
        }


    }
}
