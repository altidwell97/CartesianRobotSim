using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Commands
{
    public abstract class AsyncCommandBase : CommandBase
    {
        private bool _isExecuting;
        public bool IsExcuting
        {
            get
            {
                return _isExecuting;
            }
            set
            {
                _isExecuting = value;
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !IsExcuting && base.CanExecute(parameter);
        }

        public override async void Execute(object? parameter)
        {
            IsExcuting = true;

            try
            {
                await ExecuteAsync(parameter);
            }
            finally
            {
                IsExcuting = false;
            }
        }

        public abstract Task ExecuteAsync(object? parameter);
    }
}
