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

        public void AddVertex(Vertex vertex)
        {
            if (vertex == null) throw new ArgumentNullException(nameof(vertex));

            // If there is a last vertex, disallow adding an identical one in sequence
            if (_path.Count > 0)
            {
                var last = _path[_path.Count - 1];
                if (last.XValue == vertex.XValue && last.YValue == vertex.YValue && last.ZValue == vertex.ZValue)
                {
                    throw new ArgumentException("Vertex is already the last vertex in the path.");
                }
            }

            if (_path.Count == 5)
            {
                throw new InvalidOperationException("Path cannot contain more than 5 vertices.");
            }

            _path.Add(vertex);
        }
    }
}
