using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MarsRobots.Entities
{
    /// <summary>
    /// Exception for lost robots.
    /// </summary>
    public class LostRobotException : Exception
    {
        private Position lastKnownPosition = null;

        public override string Message => $"Lost Robot - Last known position (x: {lastKnownPosition.X}, y: {lastKnownPosition.Y}, Dir: {lastKnownPosition.Direction})";

        public LostRobotException(Position lastKnownPosition)
        {
            this.lastKnownPosition = lastKnownPosition;
        }
    }
}
