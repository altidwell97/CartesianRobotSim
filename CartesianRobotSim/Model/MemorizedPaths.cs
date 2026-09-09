using CartesianRobotSim.Services.JsonInteractionServices.PathCreator;
using CartesianRobotSim.Services.JsonInteractionServices.PathProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Model
{
    public class MemorizedPaths
    {
        private readonly IPathProvider _memorizedPathProvider;
        private readonly IPathCreator _memorizedPathCreator;

        public MemorizedPaths(IPathProvider memorizedPathProvider, IPathCreator memorizedPathCreator)
        {
            _memorizedPathProvider = memorizedPathProvider;
            _memorizedPathCreator = memorizedPathCreator;
        }

        public async Task AddPath(Path path) 
        { 
            await _memorizedPathCreator.CreatePath(path);
        }

        public async Task<IEnumerable<Path>> GetAllPaths()
        {
            return await _memorizedPathProvider.GetAllPaths();
        }
    }
}
