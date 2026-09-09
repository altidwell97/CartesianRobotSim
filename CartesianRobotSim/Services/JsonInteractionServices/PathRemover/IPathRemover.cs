using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CartesianRobotSim.Model;
using Path = CartesianRobotSim.Model.Path;

namespace CartesianRobotSim.Services.JsonInteractionServices.PathRemover
{
    public interface IPathRemover
    {
        Task RemovePath(Path path);
    }
}
