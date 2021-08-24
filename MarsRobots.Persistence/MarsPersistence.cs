using LiteDB;
using MarsRobots.Entities;
using System;
using System.Collections.Generic;

namespace MarsRobots.Persistence
{
    public class MarsPersistence
    {
        LiteDatabase db;
        ILiteCollection<MarsMap> maps;
        ILiteCollection<Robot> robots;

        public MarsPersistence(string dbFileName)
        {
            db = new LiteDatabase(dbFileName);
            maps = db.GetCollection<MarsMap>();
            robots = db.GetCollection<Robot>();
        }

        public bool InsertRobot(Robot robot)
        {
            try
            {
                robots.Insert(robot);
                return true;
            }
            catch(Exception ex)
            {
                //TODO Log.
                return false;
            }
        }

        public bool UpdateRobot(Robot robot)
        {
            try
            {
                robots.Update(robot);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteRobot(Robot robot)
        {
            try
            {
                robots.Delete(robot.Id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertMap(MarsMap map)
        {
            try
            {
                maps.Insert(map);
                return true;
            }
            catch (Exception ex)
            {
                //TODO Log.
                return false;
            }
        }

        public bool UpdateMap(MarsMap map)
        {
            try
            {
                maps.Update(map);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteMap(MarsMap map)
        {
            try
            {
                maps.Delete(map.Id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
