using System.Threading;

namespace CartesianRobotSim.Services.AppCancellation
{
    public interface IAppCancellationService
    {
        CancellationToken Token { get; }
        void Cancel();
    }
}
