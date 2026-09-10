using CartesianRobotSim.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartesianRobotSim.Commands
{
    public class FollowPathCommand : CommandBase
    {
        private readonly AnimatePathService _animatePathService;

        public FollowPathCommand(AnimatePathService animatePathService)
        {
            _animatePathService = animatePathService;
        }

        public override void Execute(object? parameter)
        {
            _animatePathService.Animate();
        }
    }
}
