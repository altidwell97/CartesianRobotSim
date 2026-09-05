using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CartesianRobotSim.Services.JsonInteractionServices
{
    public interface IPathCreator
    {
        Task CreatePath(Path path);
    }
}
