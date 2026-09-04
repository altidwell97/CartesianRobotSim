using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Model
{
    public class Path
    {
        private readonly List<Vertex> _path;

        public Path(List<Vertex> path)
        {
            _path = path;
        }


        public List<Vertex> GetPath()
        {
            return _path;
        }

       

        public void AddVertex(Vertex vertex) 
        {
            if (vertex == _path.ElementAt(_path.Count - 1))
            {
                throw new ArgumentException("Vertex is already the last vertex in the path.");
            }
            if(_path.Count == 5) {
                throw new InvalidOperationException("Path cannot contain more than 5 vertices.");
            }
            _path.Add(vertex);
        }
    }
}
