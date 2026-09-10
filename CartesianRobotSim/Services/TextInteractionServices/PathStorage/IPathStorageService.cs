using CartesianRobotSim.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartesianRobotSim.Services.TextInteractionServices.PathStorage
{
    public interface IPathStorageService
    {
        Task<IEnumerable<Path>> ReadAllPathsAsync();
        Task AppendPathAsync(Path path);
        Task RemovePathAsync(Path path);
    }
}
