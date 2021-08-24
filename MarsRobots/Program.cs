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
            Console.WriteLine("Hello Mars!");
            Console.WriteLine("");
            Console.WriteLine("");
            Business.MarsRobotsHelper manager = new Business.MarsRobotsHelper();

            // CONFIG MAP
            Console.WriteLine("Enter Map Config Bound Params: X Y [Name]");
            Position configMap = ReadMapConfig(Console.ReadLine());
            while (configMap == null)
            {
                Console.WriteLine(" ## Invalid map config string. Try again!");
                configMap = ReadMapConfig(Console.ReadLine());
            }
            MarsMap map = manager.CreateNewMap(configMap.X, configMap.Y);

            do
            {
                // CONFIG ROBOT
                Console.WriteLine("Enter Robot Config Params: X Y Orientation [Name]");
                Position configRobot = ReadRobotConfig(Console.ReadLine());
                while (configRobot == null)
                {
                    Console.WriteLine(" ## Invalid Robot config string. Try again!");
                    configRobot = ReadRobotConfig(Console.ReadLine());
                }
                Console.WriteLine("Enter Robot Instructions String (L, R, F) :");
                string instructionsString = Console.ReadLine();

                Robot robot = manager.CreateRobot(configRobot.X, configRobot.Y, configRobot.direction, new System.Collections.Generic.List<string>() { instructionsString });
                robot.SetMap(map);
                manager.RunRobot(robot);
                
                Console.WriteLine($"OUTPUT {robot.CurrentPosition.X} {robot.CurrentPosition.Y} {robot.CurrentPosition.direction.ToString()} {(robot.isLost ? "LOST" : string.Empty)}");
                DrawMap(robot.CurrentMap);

            } while (!exit);
        }

        private static Position ReadMapConfig(string inputLine)
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

        private static Position ReadRobotConfig(string inputLine)
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
                        string mes = info.LostRobots ? " [X]" : info.Explored ? " [·]" : " [ ]";
                        if (x == 0)
                            mes = $"{y.ToString("00")}    {mes}";
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
    }
}
