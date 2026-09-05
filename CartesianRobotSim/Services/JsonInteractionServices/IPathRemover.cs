using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CartesianRobotSim.Services.JsonInteractionServices
{
    public interface IPathRemover
    {
        Task RemovePath(Path path);
    }
}
