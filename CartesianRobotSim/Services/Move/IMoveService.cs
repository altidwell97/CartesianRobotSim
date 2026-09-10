using System.Threading.Tasks;

namespace CartesianRobotSim.Services.Move
{
    public interface IMoveService
    {
        Task MoveToAsync(double x, double y, double z);
    }
}
