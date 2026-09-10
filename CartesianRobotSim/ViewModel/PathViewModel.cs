using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Path = CartesianRobotSim.Model.Path;

namespace CartesianRobotSim.ViewModel
{
    public class PathViewModel
    {
        private readonly Path _path;
        public int Index { get; }

        public PathViewModel(Path path, int index)
        {
            _path = path;
            Index = index;
        }

        public Path GetPath() => _path;

        public System.Collections.Generic.IList<Model.Vertex> Vertices => _path.GetPath();

        private static string FormatVertex(Model.Vertex v)
        {
            if (v == null) return string.Empty;
            return $"({v.XValue:0.###},{v.YValue:0.###},{v.ZValue:0.###})";
        }

        public string Label => $"Path {Index}";

        public string Vertex1 => _path.GetPath().Count >= 1 ? FormatVertex(_path.GetPath()[0]) : string.Empty;
        public string Vertex2 => _path.GetPath().Count >= 2 ? FormatVertex(_path.GetPath()[1]) : string.Empty;
        public string Vertex3 => _path.GetPath().Count >= 3 ? FormatVertex(_path.GetPath()[2]) : string.Empty;
        public string Vertex4 => _path.GetPath().Count >= 4 ? FormatVertex(_path.GetPath()[3]) : string.Empty;
        public string Vertex5 => _path.GetPath().Count >= 5 ? FormatVertex(_path.GetPath()[4]) : string.Empty;
    }
}
