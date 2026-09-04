using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.ViewModel
{
    public class VertexViewModel : ViewModelBase
    {
        private readonly Vertex _vertex;

        public double XValue => _vertex.XValue;
        public double YValue => _vertex.YValue;
        public double ZValue => _vertex.ZValue;

        public VertexViewModel(Vertex vertex)
        {
            _vertex = vertex;
        }
    }
}
