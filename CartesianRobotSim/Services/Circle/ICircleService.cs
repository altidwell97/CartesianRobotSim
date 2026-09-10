using System.Threading.Tasks;

namespace CartesianRobotSim.Services.Circle
{
    public interface ICircleService
    {
        Task AnimateCircleAsync(string axis, double radius);
    }
}
