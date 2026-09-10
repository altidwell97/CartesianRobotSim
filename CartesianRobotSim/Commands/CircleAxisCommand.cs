using ICircleService = CartesianRobotSim.Services.Circle.ICircleService;
using CartesianRobotSim.ViewModel;
using System;
using System.Threading.Tasks;

namespace CartesianRobotSim.Commands
{
    public class CircleAxisCommand : AsyncCommandBase
    {
        private readonly ICircleService _circleService;

        public CircleAxisCommand(ICircleService circleService)
        {
            _circleService = circleService ?? throw new ArgumentNullException(nameof(circleService));
        }

        /// <summary>
        /// Determines whether the command can execute based on the provided parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object? parameter)
        {
            // Allow execution when a CommandControlsViewModel is supplied
            if (parameter is CommandControlsViewModel) return true;
            return base.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command asynchronously, animating a circle based on the selected axis and radius from the provided ViewModel.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is not CommandControlsViewModel vm) return;

            var axis = vm.SelectedAxis.ToString();
            var radius = vm.ZRadius;

            try
            {
                await _circleService.AnimateCircleAsync(axis, radius).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Preserve exception logging but keep it minimal
                try { Services.Logging.StartupLogger.LogException(ex); } catch { }
                throw;
            }
        }
    }
}
