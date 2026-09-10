using CartesianRobotSim.Commands;
using CartesianRobotSim.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CartesianRobotSim.ViewModel
{
    public class MemorizedPositionsViewModel : ViewModelBase, IDisposable
    {
        private readonly Stores.MemorizedPathsStore _memorizedPathsStore;
        private readonly ObservableCollection<PathViewModel> _paths;

        public IEnumerable<PathViewModel> Paths => _paths;
        public ICommand RunCommand { get; }
        public ICommand RemovePathCommand { get; }

        private PathViewModel? _selectedPath;
        public PathViewModel? SelectedPath
        {
            get => _selectedPath;
            set { _selectedPath = value; OnPropertyChanged(nameof(SelectedPath)); }
        }


        private readonly MoveToVertexCommand _moveToVertexCommand;
        private readonly RemovePathCommand _removePathCommand;

        public MemorizedPositionsViewModel(Stores.MemorizedPathsStore memorizedPathsStore, MoveToVertexCommand moveToVertexCommand, RemovePathCommand removePathCommand)
        {
            _memorizedPathsStore = memorizedPathsStore ?? throw new ArgumentNullException(nameof(memorizedPathsStore));
            _moveToVertexCommand = moveToVertexCommand ?? throw new ArgumentNullException(nameof(moveToVertexCommand));
            _removePathCommand = removePathCommand ?? throw new ArgumentNullException(nameof(removePathCommand));

            _paths = new ObservableCollection<PathViewModel>();
            int idx = 1;
            foreach (var path in _memorizedPathsStore.Paths)
            {
                _paths.Add(new PathViewModel(path, idx));
                idx++;
            }

            // Update when new paths are added (marshal to UI thread if necessary)
            _memorizedPathsStore.PathAdded += OnPathAdded;

            // If the store loads later, repopulate the list
            _memorizedPathsStore.PathsLoaded += OnPathsLoaded;

            // RunCommand - animate the cursor down the selected path
            RunCommand = new RelayCommand(async _ =>
            {
                if (SelectedPath == null) return;

                var verts = SelectedPath.Vertices;
                foreach (var v in verts)
                {
                    await _moveToVertexCommand.ExecuteAsync(v).ConfigureAwait(false);
                }
            });

            // Use DI-provided RemovePathCommand; it expects a PathViewModel as the command parameter
            RemovePathCommand = _removePathCommand;
        }

        private void OnPathAdded(Model.Path path)
        {
            var dispatcher = System.Windows.Application.Current?.Dispatcher;
            void AddPath()
            {
                int next = _paths.Count + 1;
                _paths.Add(new PathViewModel(path, next));
            }

            if (dispatcher == null || dispatcher.CheckAccess())
            {
                AddPath();
            }
            else
            {
                dispatcher.Invoke(AddPath);
            }
        }

        private void OnPathsLoaded(System.Collections.Generic.IEnumerable<Model.Path> paths)
        {
            var dispatcher = System.Windows.Application.Current?.Dispatcher;
            void Populate()
            {
                _paths.Clear();
                int i = 1;
                foreach (var p in paths)
                {
                    _paths.Add(new PathViewModel(p, i));
                    i++;
                }
            }

            if (dispatcher == null || dispatcher.CheckAccess())
            {
                Populate();
            }
            else
            {
                dispatcher.Invoke(Populate);
            }
        }

        public void Dispose()
        {
            try
            {
                _memorizedPathsStore.PathAdded -= OnPathAdded;
            }
            catch { }
            try
            {
                _memorizedPathsStore.PathsLoaded -= OnPathsLoaded;
            }
            catch { }
        }
    }
}
