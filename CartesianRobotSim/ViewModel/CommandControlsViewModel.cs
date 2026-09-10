using System;
using System.Windows.Input;
using CartesianRobotSim.Model;
using CartesianRobotSim.Commands;
using CartesianRobotSim.ViewModel;

namespace CartesianRobotSim.ViewModel
{
    public class CommandControlsViewModel : ViewModelBase
    {
        private double _xDistance;
        public double XDistance
        {
            get => _xDistance;
            set
            {
                _xDistance = value;
                OnPropertyChanged(nameof(XDistance));
            }
        }

        private double _yDistance;
        public double YDistance
        {
            get => _yDistance;
            set
            {
                _yDistance = value;
                OnPropertyChanged(nameof(YDistance));
            }
        }

        private double _zDistance;
        public double ZDistance
        {
            get => _zDistance;
            set
            {
                _zDistance = value;
                OnPropertyChanged(nameof(ZDistance));
            }
        }

        private double _zRadius;
        public double ZRadius
        {
            get => _zRadius;
            set
            {
                _zRadius = value;
                OnPropertyChanged(nameof(ZRadius));
            }
        }

        private Axis _selectedAxis = Axis.Z;
        public Axis SelectedAxis
        {
            get => _selectedAxis;
            set
            {
                _selectedAxis = value;
                OnPropertyChanged(nameof(SelectedAxis));
            }
        }

        public ICommand MoveCommand { get; }
        public ICommand CircleCommand { get; }

        private readonly MoveToVertexCommand _moveToVertexCommand;
        private readonly CartesianRobotSim.Commands.CircleAxisCommand _circleAxisCommand;

        /// <summary>
        /// Initializes the MoveCommand to the moveToVertexCOmmand and the CircleCommand to thecircleAxisCommand so that the infromation from the CommandControl view
        /// can be passed on and animated in the Robot Environment view appropriately. The MoveCommand moveToVertexCommand passes the points entered in the text boxes
        /// to the MoveToVertexCommand classes execute function so that the movement can be animated. The circleAxisCommand passes to CircleAxisCommand class to handle 
        /// animating circling around the desired axis in the RobotEnvironment view.
        /// </summary>
        /// <param name="moveToVertexCommand"></param>
        /// <param name="circleAxisCommand"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CommandControlsViewModel(MoveToVertexCommand moveToVertexCommand, CircleAxisCommand circleAxisCommand)
        {
            _moveToVertexCommand = moveToVertexCommand ?? throw new ArgumentNullException(nameof(moveToVertexCommand));
            _circleAxisCommand = circleAxisCommand ?? throw new ArgumentNullException(nameof(circleAxisCommand));

            MoveCommand = new RelayCommand(_ => _moveToVertexCommand.Execute((XDistance, YDistance, ZDistance)));

            CircleCommand = _circleAxisCommand;
        }
    }
}
