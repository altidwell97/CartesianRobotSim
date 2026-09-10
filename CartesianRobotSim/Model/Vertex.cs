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

        /// <summary>
        /// Returns a string representation of the vertex with invariant culture and 3 decimal places.
        /// </summary>
        /// <returns>A string in the format "(XValue,YValue,ZValue)"</returns>
        public override string ToString()
        {
            // Use invariant culture and 3 decimal places to match other formatting in the app
            return $"({XValue:0.###},{YValue:0.###},{ZValue:0.###})";
        }
    }
}
