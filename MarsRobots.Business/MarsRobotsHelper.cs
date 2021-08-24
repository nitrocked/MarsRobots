using LiteDB;
using MarsRobots.Entities;
using MarsRobots.Entities.Enums;
using MarsRobots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MarsRobots.Business
{
    public class MarsRobotsHelper
    {
        private int maxCoordinate = 50;
        private int maxInstructionsSrtings = 1;
        private int maxIntructionStringLenght = 100;
        private string databaseFileName = "marsDB.db";

        MarsPersistence persistence;

        private List<MarsMap> maps = new List<MarsMap>();
        private List<Robot> robots = new List<Robot>();

        public MarsRobotsHelper()
        {
            try
            {
                persistence = new MarsPersistence(databaseFileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates a new map and persist it to DB.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public MarsMap CreateNewMap(int x, int y)
        {
            if (x < 1 || y < 1 || x > maxCoordinate || y > maxCoordinate) throw new ArgumentException(String.Format(MarsResources.BL_Error_MapCoordinatesBounds, maxCoordinate));

            MarsMap newMap = null;

            try
            {
                if (maps != null)
                    maps = new List<MarsMap>();

                newMap = new MarsMap(x, y);

                if (persistence.InsertMap(newMap))
                    maps.Add(newMap);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initX"></param>
        /// <param name="initY"></param>
        /// <param name="direction"></param>
        /// <param name="instructionsStrings"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Robot CreateRobot(int initX, int initY, CardinalDirection direction, List<string> instructionsStrings, string name = "")
        {
            if (instructionsStrings.Count > maxInstructionsSrtings) throw new ArgumentOutOfRangeException("instructionsStrings");

            Robot newRobot = null;

            try
            {
                newRobot = new Robot(initX, initY, direction, instructionsStrings, name);
                persistence.InsertRobot(newRobot);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newRobot;
        }

        /// <summary>
        /// Runs the InstructionsStrings stored in the robot.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="robot"></param>
        /// <returns></returns>
        public bool RunRobot(Robot robot)
        {
            bool res = false;
            try
            {
                if (robot != null && robot.InstructionStrings != null)
                {
                    foreach (string instructionString in robot.InstructionStrings)
                    {
                        List<RobotAction> actions = this.InputStringToActionList(instructionString);
                        foreach (RobotAction action in actions)
                        {
                            robot.Execute(action);
                        }
                        //actions.ForEach(a => robot.Execute(a));
                    }
                    res = true;
                }
            }
            catch (LostRobotException lex)
            {
                //Log
                //throw lex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //After all, save the state of robot.
                persistence.UpdateRobot(robot);
                persistence.UpdateMap(robot.CurrentMap);
            }

            return res;
        }

        /// <summary>
        /// Get a list of actions from a string.
        /// L -> Left
        /// R -> Right
        /// F -> forward
        /// </summary>
        /// <param name="commandString"></param>
        /// <returns></returns>
        private List<RobotAction> InputStringToActionList(string commandString)
        {
            List<RobotAction> actions = new List<RobotAction>();

            if (!string.IsNullOrEmpty(commandString))
            {
                foreach (char item in commandString.ToList())
                {
                    if (item.ToString().ToUpper() == "L")
                    {
                        actions.Add(RobotAction.Left);
                    }
                    else if (item.ToString().ToUpper() == "R")
                    {
                        actions.Add(RobotAction.Right);
                    }
                    else if (item.ToString().ToUpper() == "F")
                    {
                        actions.Add(RobotAction.Forward);
                    }
                }
            }

            return actions;
        }
    }
}
