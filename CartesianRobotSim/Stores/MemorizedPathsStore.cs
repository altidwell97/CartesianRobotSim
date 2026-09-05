using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Stores
{
    public class MemorizedPathsStore
    {
        private readonly List<Path> _paths;
        private readonly MemorizedPaths _memorizedPaths;

        public IEnumerable<Path> Paths => _paths;

        public event Action<Path> PathAdded;

        public MemorizedPathsStore(MemorizedPaths memorizedPaths)
        {
            _paths = new List<Path>();
            _memorizedPaths = memorizedPaths;
        }

        public async Task Load()
        {
            IEnumerable<Path> paths = await _memorizedPaths.GetAllPaths();
        }
    }
}
