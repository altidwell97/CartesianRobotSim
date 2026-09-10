using CartesianRobotSim.ViewModel;
using System;
using System.Threading.Tasks;

namespace CartesianRobotSim.Services.Circle
{
    public class CircleService : ICircleService
    {
        private readonly RobotEnvironmentViewModel _environment;
        private readonly System.Threading.CancellationToken _appToken;

        public CircleService(RobotEnvironmentViewModel environment, CartesianRobotSim.Services.AppCancellation.IAppCancellationService cancellation)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _appToken = cancellation?.Token ?? System.Threading.CancellationToken.None;
        }

        public async Task AnimateCircleAsync(string axis, double radius)
        {
            // Use current pointer as center
            double centerX = _environment.PointerX;
            double centerY = _environment.PointerY;
            double centerZ = _environment.PointerZ;

            if (radius <= 0) return;

            int steps = 120;
            int delayMs = 10;

            var dispatcher = System.Windows.Application.Current?.Dispatcher;

            for (int i = 0; i <= steps; i++)
            {
                if (_appToken.IsCancellationRequested) break;
                double angle = 2.0 * Math.PI * i / steps;
                double x = centerX;
                double y = centerY;
                double z = centerZ;

                switch (axis?.ToUpperInvariant())
                {
                    case "X":
                        y = centerY + radius * Math.Cos(angle);
                        z = centerZ + radius * Math.Sin(angle);
                        break;
                    case "Y":
                        x = centerX + radius * Math.Cos(angle);
                        z = centerZ + radius * Math.Sin(angle);
                        break;
                    default:
                        x = centerX + radius * Math.Cos(angle);
                        y = centerY + radius * Math.Sin(angle);
                        break;
                }

                if (dispatcher != null)
                {
                    dispatcher.InvokeAsync(() =>
                    {
                        _environment.PointerX = x;
                        _environment.PointerY = y;
                        _environment.PointerZ = z;
                    }, System.Windows.Threading.DispatcherPriority.Render);
                }
                else
                {
                    _environment.PointerX = x;
                    _environment.PointerY = y;
                    _environment.PointerZ = z;
                }

                try { await Task.Delay(delayMs, _appToken).ConfigureAwait(false); } catch (OperationCanceledException) { break; }
            }
        }
    }
}
