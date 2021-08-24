using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRobots.Entities
{
    public class PositionInfo
    {
        /// <summary>
        /// Indicates this point has been explored by a robot.
        /// </summary>
        public bool Explored { get; set; }

        /// <summary>
        /// inidicates some robot has been lost from this position.
        /// </summary>
        public bool LostRobots { get; set; }

        //Default constructor.
        public PositionInfo()
        {
            this.Explored = this.LostRobots = false;
        }

    }
}
