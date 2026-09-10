using System.Threading;

namespace CartesianRobotSim.Services.AppCancellation
{
    public class AppCancellationService : IAppCancellationService
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public CancellationToken Token => _cts.Token;

        public void Cancel() => _cts.Cancel();
    }
}
