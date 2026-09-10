using CartesianRobotSim.ViewModel;
using System;
using System.Threading.Tasks;

namespace CartesianRobotSim.Services.Move
{
    public class MoveService : IMoveService
    {
        private readonly RobotEnvironmentViewModel _environment;
        private readonly System.Threading.CancellationToken _appToken;

        // Duration of the animation in milliseconds
        private readonly int _durationMs = 500;

        public MoveService(RobotEnvironmentViewModel environment, CartesianRobotSim.Services.AppCancellation.IAppCancellationService cancellation)
        {
            _environment = environment;
            _appToken = cancellation?.Token ?? System.Threading.CancellationToken.None;
        }

        public async Task MoveToAsync(double x, double y, double z)
        {
            // Capture start values
            double startX = _environment.PointerX;
            double startY = _environment.PointerY;
            double startZ = _environment.PointerZ;

            double deltaX = x - startX;
            double deltaY = y - startY;
            double deltaZ = z - startZ;

            if (_durationMs <= 0)
            {
                _environment.PointerX = x;
                _environment.PointerY = y;
                _environment.PointerZ = z;
                return;
            }

            int steps = Math.Max(1, _durationMs / 15);
            var dispatcher = System.Windows.Application.Current?.Dispatcher;

            for (int i = 1; i <= steps; i++)
            {
                if (_appToken.IsCancellationRequested) break;
                double t = (double)i / steps;
                // Simple ease-in-out interpolation (smoothstep)
                double tt = t * t * (3 - 2 * t);

                double nextX = startX + deltaX * tt;
                double nextY = startY + deltaY * tt;
                double nextZ = startZ + deltaZ * tt;

                // Update on UI thread with Render priority to reduce layout churn and keep animation smooth
                if (dispatcher != null)
                {
                    dispatcher.InvokeAsync(() =>
                    {
                        _environment.PointerX = nextX;
                        _environment.PointerY = nextY;
                        _environment.PointerZ = nextZ;
                    }, System.Windows.Threading.DispatcherPriority.Render);
                }
                else
                {
                    _environment.PointerX = nextX;
                    _environment.PointerY = nextY;
                    _environment.PointerZ = nextZ;
                }

                // delay off the UI thread to avoid blocking renders
                try { await Task.Delay(15, _appToken).ConfigureAwait(false); } catch (OperationCanceledException) { break; }
            }

            // Ensure final values
            _environment.PointerX = x;
            _environment.PointerY = y;
            _environment.PointerZ = z;
        }
    }
}
