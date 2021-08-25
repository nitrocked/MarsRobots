using MarsRobots.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Linq;

namespace MarsRobots.Entities
{
    public class Robot : IRobot
    {
        /// <summary>
        /// Unique Identifier for the robot.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of robot.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initial position of the robot in map.
        /// </summary>
        public Position InitialPosition { get; private set; }

        /// <summary>
        /// Current position of the robor in the map.
        /// </summary>
        public Position CurrentPosition { get; set; }

        /// <summary>
        /// Flag indicating the robot is lost: out of map bounds.
        /// </summary>
        public bool IsLost { get; set; }

        /// <summary>
        /// Set of instructions strings for the robot.
        /// </summary>
        public List<string> InstructionStrings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MarsMap CurrentMap { get; private set; }

        /// <summary>
        /// Logs all executed action of robot from initial moment or last reset.
        /// </summary>
        public List<Position> Tracking { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Robot()
        {
            this.InitialPosition = new Position();
            this.CurrentPosition = this.InitialPosition.Copy();
            this.CurrentMap = null;
            this.Tracking = new List<Position>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initX"></param>
        /// <param name="initY"></param>
        /// <param name="direction"></param>
        /// <param name="instructionsStrings">List of instructions strings</param>
        /// <param name="name">Optional. Name of Robot.</param>
        public Robot(int initX, int initY, CardinalDirection direction, List<string> instructionsStrings, string name = "")
        {
            // initially, current position is the initial position.
            this.InitialPosition = new Position(initX, initY, direction);
            this.CurrentPosition = this.InitialPosition.Copy();
            this.InstructionStrings = instructionsStrings;
            this.Name = name;
            this.Tracking = new List<Position>();
        }

        /// <summary>
        /// Asigns the map to the robot and resets the position to initial position.
        /// </summary>
        /// <param name="map"></param>
        public void SetMap(MarsMap map)
        {
            if (map == null) throw new ArgumentNullException("map");

            this.CurrentMap = map;
            this.ResetRobot();
        }

        /// <summary>
        /// Resets the robot and set the current position to initial position.
        /// </summary>
        /// <returns></returns>
        public bool ResetRobot()
        {
            this.CurrentPosition = this.InitialPosition.Copy();
            this.IsLost = false;
            this.Tracking = new List<Position>();
            return true;
        }

        /// <summary>
        /// Executes an action into the robot.
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool Execute(RobotAction action)
        {
            bool res = false;
            try
            {
                if (this.InitialPosition != null && this.CurrentPosition != null && !this.IsLost)
                {
                    switch (action)
                    {
                        case RobotAction.Left:
                            this.CurrentPosition.Left();
                            this.Tracking.Add(this.CurrentPosition.Copy());
                            break;
                        case RobotAction.Right:
                            this.CurrentPosition.Right();
                            this.Tracking.Add(this.CurrentPosition.Copy());
                            break;
                        case RobotAction.Forward:
                            this.CurrentMap.GetPositionInfo(this.CurrentPosition).SetExplored();

                            if (!this.HasLostRobotsTowardsCurrentDirection())
                            {
                                if (this.IsForwardInsideMap())
                                {
                                    this.CurrentPosition.Forward();
                                    this.Tracking.Add(this.CurrentPosition.Copy());
                                }
                                else
                                {
                                    this.CurrentMap.GetPositionInfo(this.CurrentPosition).AddForbiddenDirection(CurrentPosition.Direction);
                                    //this.CurrentMap.SetFence(this.CurrentPosition.Direction);
                                    this.IsLost = true;
                                    throw new LostRobotException(this.CurrentPosition.Copy());
                                }
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO implement some logging strategy.
                res = false;
                throw ex;
            }

            return res;
        }

        /// <summary>
        /// Checks if the next position in map to go forward is inside map bounds.
        /// </summary>
        /// <returns></returns>
        private bool IsForwardInsideMap()
        {
            try
            {
                if (this.CurrentMap != null && this.CurrentPosition != null)
                {
                    Position aux = this.CurrentPosition.Copy();

                    aux.Forward();

                    return this.CurrentMap.IsPointInsideMap(aux);
                }
            }
            catch (IndexOutOfRangeException orex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Checks if the next grid point forward movement were lost robots.
        /// </summary>
        /// <returns></returns>
        private bool HasLostRobotsTowardsCurrentDirection()
        {
            try
            {
                if (this.CurrentMap != null && this.CurrentPosition != null)
                {
                    return this.CurrentMap.GetPositionInfo(this.CurrentPosition).ForbiddenDirections.Count(d => d == this.CurrentPosition.Direction) > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }
}
