using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Services.JsonInteractionServices.PathCreator
{
    public interface IPathCreator
    {
        Task CreatePath(Path path);
    }
}
