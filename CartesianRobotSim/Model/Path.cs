using System;
using System.Collections.Generic;

namespace CartesianRobotSim.Model
{
    public class Path
    {
        private readonly List<Vertex> _path;

        public Path(List<Vertex> path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public List<Vertex> GetPath()
        {
            return _path;
        }  
    }
}
