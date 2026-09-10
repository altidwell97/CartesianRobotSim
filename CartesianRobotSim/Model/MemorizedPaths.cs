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

        /// <summary>
        /// Uses the storage service to append a new path to the stored paths.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task AddPath(Path path)
        {
            await _storage.AppendPathAsync(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Uses the storage service to remove a path from the stored paths.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task RemovePath(Path path)
        {
            await _storage.RemovePathAsync(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Uses the storage service to retrieve all stored paths.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Path>> GetAllPaths()
        {
            return await _storage.ReadAllPathsAsync().ConfigureAwait(false);
        }
    }
}
