using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CartesianRobotSim.Stores
{
    public class MemorizedPathsStore
    {
        private readonly List<Path> _paths;
        private readonly MemorizedPaths _memorizedPaths;

        public IEnumerable<Path> Paths => _paths;

        public event Action<Path> PathAdded;
        public event Action<System.Collections.Generic.IEnumerable<Path>> PathsLoaded;
        public event Action<Path>? PathRemoved;

        public MemorizedPathsStore(MemorizedPaths memorizedPaths)
        {
            _paths = new List<Path>();
            _memorizedPaths = memorizedPaths;
        }

        public async Task Load()
        {
            IEnumerable<Path> paths = await _memorizedPaths.GetAllPaths();

            _paths.Clear();
            _paths.AddRange(paths);
            PathsLoaded?.Invoke(_paths);
        }

        public async Task AddPath(Path path)
        {
            _paths.Add(path);
            PathAdded?.Invoke(path);
            // Path added
            await _memorizedPaths.AddPath(path);
        }

        public async Task RemovePath(Path path)
        {
            // Remove from in-memory list using reference equality first, then content equality
            Path? found = _paths.FirstOrDefault(p => ReferenceEquals(p, path));
            if (found == null)
            {
                // Fall back to content equality by comparing vertex lists
                foreach (var p in _paths)
                {
                    var a = p.GetPath();
                    var b = path.GetPath();
                    if (a.Count == b.Count)
                    {
                        bool same = true;
                        for (int i = 0; i < a.Count; i++)
                        {
                            if (a[i].XValue != b[i].XValue || a[i].YValue != b[i].YValue || a[i].ZValue != b[i].ZValue)
                            {
                                same = false; break;
                            }
                        }
                        if (same) { found = p; break; }
                    }
                }
            }

            if (found != null)
            {
                _paths.Remove(found);
                PathRemoved?.Invoke(found);
                PathsLoaded?.Invoke(_paths);
            }

            // Persist removal
            await _memorizedPaths.RemovePath(path).ConfigureAwait(false);
        }
    }
}
