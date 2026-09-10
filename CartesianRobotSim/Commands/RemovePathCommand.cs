using CartesianRobotSim.Stores;
using CartesianRobotSim.ViewModel;
using System;
using System.Threading.Tasks;

namespace CartesianRobotSim.Commands
{
    // Removes a selected path from the memorized paths store (and underlying storage)
    public class RemovePathCommand : AsyncCommandBase
    {
        private readonly MemorizedPathsStore _store;

        public RemovePathCommand(MemorizedPathsStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        /// <summary>
        /// Determines whether the command can execute based on the provided parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object? parameter)
        {
            // Expect a PathViewModel as parameter or rely on store selection via VM
            if (parameter is ViewModel.PathViewModel) return true;
            return false;
        }

        /// <summary>
        /// Executes the command asynchronously, removing the selected path from the memorized paths store.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override async Task ExecuteAsync(object? parameter)
        {
            if (!(parameter is ViewModel.PathViewModel pvm)) return;

            var path = pvm.GetPath();
            await _store.RemovePath(path).ConfigureAwait(false);
        }
    }
}
