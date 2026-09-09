using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Services.JsonInteractionServices.PathProvider
{
    public interface IPathProvider
    {
        Task<IEnumerable<Path>> GetAllPaths();
    }
}
