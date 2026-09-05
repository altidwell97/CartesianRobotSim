using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Services.JsonInteractionServices
{
    public interface IPathProvider
    {
        Task<IEnumerable<Path>> GetAllPaths();
    }
}
