using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Model
{
    public class Circle
    {
        public double Radius { get; }
        public double[] Center { get; } = new double[3];

        public Circle(double radius, double[] center)
        {
            Radius = radius;
            Center = center;
        }
    }
}
