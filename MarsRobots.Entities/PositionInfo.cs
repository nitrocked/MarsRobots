using System;
using System.Collections.Generic;
using System.Text;
using MarsRobots.Entities.Enums;

namespace MarsRobots.Entities
{
    /// <summary>
    /// It doesn´t inherit from Position as Direction has no sense here.
    /// </summary>
    public class PositionInfo
    {
        /// <summary>
        /// X Coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y Coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Indicates this point has been explored by a robot.
        /// </summary>
        public bool Explored { get; set; }

        /// <summary>
        /// Robot positions that have been lost in this point.
        /// </summary>
        public List<CardinalDirection> ForbiddenDirections { get; set; }

        public PositionInfo()
        {
            this.Explored = false;
            this.ForbiddenDirections = new List<CardinalDirection>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pos"></param>
        public PositionInfo(Position pos)
        {
            this.Explored = false;
            this.ForbiddenDirections = new List<CardinalDirection>();
            this.X = pos.X;
            this.Y = pos.Y;
        }

        /// <summary>
        /// Adds a forbidden direction from this point.
        /// </summary>
        /// <param name="direction"></param>
        public void AddForbiddenDirection(CardinalDirection direction)
        {
            if (!this.ForbiddenDirections.Contains(direction))
                this.ForbiddenDirections.Add(direction);
        }

        /// <summary>
        /// Set the current position as explored. 
        /// Used by private setter for future more complex logic.
        /// </summary>
        public void SetExplored()
        {
            this.Explored = true;
        }
    }
}
