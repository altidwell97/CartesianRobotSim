using CartesianRobotSim.Services.TextInteractionServices.PathStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Model
{
    public class MemorizedPaths
    {
        private readonly IPathStorageService _storage;

        public MemorizedPaths(IPathStorageService storage)
        {
            _storage = storage;
        }

        public async Task AddPath(Path path)
        {
            await _storage.AppendPathAsync(path).ConfigureAwait(false);
        }

        public async Task RemovePath(Path path)
        {
            await _storage.RemovePathAsync(path).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Path>> GetAllPaths()
        {
            return await _storage.ReadAllPathsAsync().ConfigureAwait(false);
        }
    }
}
