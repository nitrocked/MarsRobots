using MarsRobots.Entities;
using MarsRobots.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace MarsRobots
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            string userInput = string.Empty;
            Console.WriteLine("Hello Mars!");
            Console.WriteLine("");
            Console.WriteLine("");
            Business.MarsRobotsHelper manager = new Business.MarsRobotsHelper();

            // CONFIG MAP
            Console.WriteLine("Enter Map Config Bound Params: X Y [Name]");
            userInput = Console.ReadLine();
            Position configMap = manager.ReadMapConfig(userInput);

            while (configMap == null)
            {
                Console.WriteLine(" ## Invalid map config string. Try again!");
                userInput = Console.ReadLine();
                configMap = manager.ReadMapConfig(userInput);
            }
            MarsMap map = manager.CreateNewMap(configMap.X, configMap.Y);

            do
            {
                // CONFIG ROBOT
                Console.WriteLine("Enter Robot Config Params: X Y Orientation [Name]");
                userInput = Console.ReadLine();
                if (RequestExit(userInput)) 
                    return;
                Position configRobot = manager.ReadRobotConfig(userInput);

                while (configRobot == null)
                {
                    Console.WriteLine(" ## Invalid Robot config string. Try again!");
                    userInput = Console.ReadLine();
                    if (RequestExit(userInput)) 
                        return;
                    configRobot = manager.ReadRobotConfig(userInput);
                }
                Console.WriteLine("Enter Robot Instructions String (L, R, F) :");
                userInput = Console.ReadLine();
                if (RequestExit(userInput)) 
                    return;

                try
                {
                    Robot robot = manager.CreateRobot(configRobot.X, configRobot.Y, configRobot.Direction, new System.Collections.Generic.List<string>() { userInput });
                    
                    robot.SetMap(map);
                    manager.RunRobot(robot);

                    Console.WriteLine($"OUTPUT {robot.CurrentPosition.X} {robot.CurrentPosition.Y} {robot.CurrentPosition.Direction.ToString()} {(robot.IsLost ? "LOST" : string.Empty)}");
                    DrawMap(robot.CurrentMap);
                }
                catch(LostRobotException lrex)
                {
                    Console.WriteLine(lrex.Message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (!exit);
        }

        /// <summary>
        /// Console basic map info drawing.
        /// </summary>
        /// <param name="map"></param>
        private static void DrawMap(MarsMap map)
        {
            List<string> lines = new List<string>();
            if (map != null)
            {
                string axisX = " Y X ";
                for (int y = 0; y < map.SizeY; y++)
                {
                    string line = string.Empty;
                    for (int x = 0; x < map.SizeX; x++)
                    {
                        if (y == 0)
                            axisX += String.Format($" {x.ToString("00")} ");
                        Position curPos = new Position(x, y, CardinalDirection.N);
                        PositionInfo info = map.GetPositionInfo(curPos);
                        string mes = info.ForbiddenDirections.Count() > 0 ? " [X]" : info.Explored ? " [·]" : " [ ]";
                        if (x == 0)
                            mes = $"{y.ToString("00")}   {mes}";
                        line += mes;
                    }
                    lines.Add(line);
                }

                //Invert Draw .
                lines.Reverse();
                lines.Add(axisX);

                lines.ForEach(l => Console.WriteLine(l));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputLine"></param>
        /// <returns></returns>
        private static bool RequestExit(string inputLine)
        {
            return !string.IsNullOrEmpty(inputLine) && inputLine.ToUpper().Contains("Q");
        }
    }
}
