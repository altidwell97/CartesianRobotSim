using CartesianRobotSim.Model;
using CartesianRobotSim.Services.Move;
using System;

namespace CartesianRobotSim.Commands
{
    public class MoveToVertexCommand : AsyncCommandBase
    {
        private readonly IMoveService _moveService;

        public MoveToVertexCommand(IMoveService moveService)
        {
            _moveService = moveService ?? throw new ArgumentNullException(nameof(moveService));
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            double x, y, z;

            if (parameter is Vertex v)
            {
                x = v.XValue; y = v.YValue; z = v.ZValue;
            }
            else if (parameter is double[] arr && arr.Length >= 3)
            {
                x = arr[0]; y = arr[1]; z = arr[2];
            }
            else if (parameter is ValueTuple<double, double, double> t)
            {
                x = t.Item1; y = t.Item2; z = t.Item3;
            }
            else
            {
                throw new ArgumentException("MoveToVertexCommand expects a Vertex, double[] (len>=3) or (double,double,double) tuple as parameter.");
            }

            await _moveService.MoveToAsync(x, y, z).ConfigureAwait(false);
        }
    }
}
