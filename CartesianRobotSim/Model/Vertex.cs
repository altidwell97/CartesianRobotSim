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

        public override string ToString()
        {
            // Format coordinates using invariant culture
            return $"({XValue.ToString(System.Globalization.CultureInfo.InvariantCulture)},{YValue.ToString(System.Globalization.CultureInfo.InvariantCulture)},{ZValue.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
        }
    }
}
