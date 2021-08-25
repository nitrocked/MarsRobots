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
        private int maxCoordinateX = 50;
        private int maxCoordinateY = 50;
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
            if (x < 0 || x > maxCoordinateX) throw new ArgumentOutOfRangeException("x", String.Format(MarsResources.BL_Error_MapCoordinatesBounds, maxCoordinateX));
            if (y < 0 || y > maxCoordinateY) throw new ArgumentOutOfRangeException("y", String.Format(MarsResources.BL_Error_MapCoordinatesBounds, maxCoordinateY));
          
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
            if (initX < 0 || initX > maxCoordinateX) throw new ArgumentOutOfRangeException("initX", String.Format(MarsResources.BL_Error_MapCoordinatesBounds, maxCoordinateX));
            if (initY < 0 || initY > maxCoordinateY) throw new ArgumentOutOfRangeException("initY", String.Format(MarsResources.BL_Error_MapCoordinatesBounds, maxCoordinateY));
            if (instructionsStrings.Count > maxInstructionsSrtings) throw new ArgumentOutOfRangeException("instructionsStrings", String.Format(MarsResources.BL_Ex_MaxInstructionStringLenght, maxIntructionStringLenght));

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
                        this.InputStringToActionList(instructionString).ForEach(a => robot.Execute(a));
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
        /// Transform string to position.
        /// Format: <X> <Y>
        /// </summary>
        /// <param name="inputLine"></param>
        /// <returns></returns>
        public Position ReadMapConfig(string inputLine)
        {
            int x = 0;
            int y = 0;
            bool res = false;
            try
            {
                if (!string.IsNullOrEmpty(inputLine))
                {
                    List<string> values = inputLine.TrimStart().TrimEnd().ToUpper().Split(" ").ToList();
                    if (values != null && values.Count == 2)
                    {
                        x = int.Parse(values[0]);
                        y = int.Parse(values[1]);
                        res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invalid cast to int or other issues.
                res = false;
            }

            if (res)
                return new Position(x, y, CardinalDirection.N);
            else
                return null;
        }

        /// <summary>
        /// Transform string into initial roboto position config.
        /// Formar: <initialX> <initialY> <[N, S, E, W]>
        /// </summary>
        /// <param name="inputLine"></param>
        /// <returns></returns>
        public Position ReadRobotConfig(string inputLine)
        {
            int x = -1;
            int y = -1;
            CardinalDirection dir = CardinalDirection.N;
            bool readOk = false;
            try
            {
                if (!string.IsNullOrEmpty(inputLine))
                {
                    List<string> values = inputLine.TrimStart().TrimEnd().ToUpper().Split(" ").ToList();
                    if (values != null && values.Count == 3)
                    {
                        x = int.Parse(values[0]);
                        y = int.Parse(values[1]);
                        dir = Enum.Parse<CardinalDirection>(values[2].ToString());
                        readOk = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invalid cast to int or other issues.
                readOk = false;
            }

            if (readOk)
                return new Position(x, y, dir);
            else
                return null;
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
