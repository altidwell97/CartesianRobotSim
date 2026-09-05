using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Model
{
    public class MemorizedPaths
    {
        private readonly List<Path> _memorizedPaths;

        public MemorizedPaths(List<Path> memorizedPaths)
        {
            _memorizedPaths = memorizedPaths;
        }

        public void AddPath(Path path) 
        { 
            _memorizedPaths.Add(path);
        }

        public async Task<IEnumerable<Path>> GetAllPaths()
        {
            return await _memorizedPaths.GetAllPaths();
        }>
    }
}
