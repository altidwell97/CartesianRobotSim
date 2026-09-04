using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CartesianRobotSim.Commands
{
    public class MakeVertexCommand : CommandBase
    {
        private readonly List<Vertex> _path;
        private readonly double _xValue;
        private readonly double _yValue;
        private readonly double _zValue;

        public MakeVertexCommand(double x, double y, double z, List<Vertex> path)
        {
            _xValue = x;
            _yValue = y;
            _zValue = z;
            _path = path;
        }

        public override bool CanExecute(object? parameter)
        {
            int length = _path.Count();
            if(length < 5)
            {
                return true;

            }
            else
            {
                MessageBox.Show("Path is full. Cannot add more vertices.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public override void Execute(object? parameter)
        {
            Vertex vertex = new Vertex(_xValue, _yValue, _zValue);

            try
            {
                _path.Add(vertex);

                MessageBox.Show("Succsessfully added vertex to path.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add vertex to path.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
