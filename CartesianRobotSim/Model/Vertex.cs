using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Model 
{
    public class Vertex
    {
        public double XValue { get; }
        public double YValue { get; }
        public double ZValue { get; }

        public Vertex(double xValue, double yValue, double zValue)
        {
            XValue = xValue;
            YValue = yValue;
            ZValue = zValue;
        }
    }
}
