using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CartesianRobotSim.Model;
using CartesianRobotSim.Commands;

namespace CartesianRobotSim.ViewModel
{
    public class PositionListEntryViewModel : ViewModelBase
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
        // Collection bound to the ListBox in the view
        public ObservableCollection<Vertex> AddedPositions { get; } = new ObservableCollection<Vertex>();

        private Vertex? _selectedPoint;
        public Vertex? SelectedPoint
        {
            get => _selectedPoint;
            set { _selectedPoint = value; OnPropertyChanged(nameof(SelectedPoint)); }
        }

        // Expose a Position property referenced by AddPathCommand
        private Vertex? _position;
        public Vertex? Position
        {
            get => _position;
            set { _position = value; OnPropertyChanged(nameof(Position)); }
        }

        private string? _addDisabledMessage;
        public string? AddDisabledMessage
        {
            get => _addDisabledMessage;
            set { _addDisabledMessage = value; OnPropertyChanged(nameof(AddDisabledMessage)); }
        }

        private string? _saveMessage;
        public string? SaveMessage
        {
            get => _saveMessage;
            set { _saveMessage = value; OnPropertyChanged(nameof(SaveMessage)); }
        }

        private string? _addMessage;
        public string? AddMessage
        {
            get => _addMessage;
            set { _addMessage = value; OnPropertyChanged(nameof(AddMessage)); }
        }

        /// <summary>
        /// Sets the AddCommand to addVertexCommand, SaveCommand to addPathCommand, and RemoveCommand to removeVertexCommand. 
        /// Attaches this VM to the commands so they can observe state and raise CanExecuteChanged.
        /// addVertexCommand is used to add a vertex to the AddedPositions collection, addPathCommand is used to save the current path, 
        /// and removeVertexCommand is used to remove a vertex from the AddedPositions collection.
        /// </summary>
        /// <param name="addVertexCommand"></param>
        /// <param name="addPathCommand"></param>
        /// <param name="removeVertexCommand"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PositionListEntryViewModel(AddVertexCommand addVertexCommand, AddPathCommand addPathCommand, RemoveVertexCommand removeVertexCommand)
        {
            if (addVertexCommand == null) throw new ArgumentNullException(nameof(addVertexCommand));
            if (addPathCommand == null) throw new ArgumentNullException(nameof(addPathCommand));
            if (removeVertexCommand == null) throw new ArgumentNullException(nameof(removeVertexCommand));

            // Use command instances directly; attach this VM so commands can observe state and raise CanExecuteChanged
            AddCommand = addVertexCommand;
            addVertexCommand.Attach(this);

            // Use the AddPathCommand directly so its CanExecute can disable the Save button when appropriate
            SaveCommand = addPathCommand;
            addPathCommand.Attach(this);

            RemoveCommand = removeVertexCommand;
            removeVertexCommand.Attach(this);
        }

        // Command enable/disable logic is handled by the command classes which observe this VM.
    }
}
