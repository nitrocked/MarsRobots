using LiteDB;
using MarsRobots.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRobots.Persistence
{
    public class MarsPersistence
    {
        LiteDatabase db;
        ILiteCollection<MarsMap> maps;
        ILiteCollection<Robot> robots;

        string dbConnectionString = string.Empty;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="dbConnectionString"></param>
        public MarsPersistence(string dbConnectionString)
        {
            try
            {
                this.dbConnectionString = dbConnectionString;
                this.db = new LiteDatabase(dbConnectionString);
                this.maps = db.GetCollection<MarsMap>("maps");
                this.robots = db.GetCollection<Robot>("robots");

                BsonMapper.Global.Entity<Robot>()
                    .Id(m => m.Id)
                    .DbRef(r => r.CurrentMap, "maps");

                BsonMapper.Global.Entity<MarsMap>()
                    .Id(m => m.Id);

                this.RegisterCustomMapper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RegisterCustomMapper()
        {
            BsonMapper.Global.RegisterType<PositionInfo[,]>
            (
                serialize: array =>
                {
                    var bsonArray = new BsonArray();
                    int I_LEN = array.GetLength(0);
                    int J_LEN = array.GetLength(1);

                    bsonArray.Add(I_LEN);
                    bsonArray.Add(J_LEN);

                    for (int i = 0; i < I_LEN; i++)
                    {
                        var arr = new BsonArray();
                        for (int j = 0; j < J_LEN; j++)
                        {
                            arr.Add(db.Mapper.Serialize<PositionInfo>(array[i, j]));
                        }
                        bsonArray.Add(arr);
                    }
                    return bsonArray;
                },
                deserialize: bson =>
                {
                    return Deserialize(bson);
                }
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bson"></param>
        /// <returns></returns>
        private PositionInfo[,] Deserialize(BsonValue bson)
        {
            var bsonArray = bson.AsArray;
            int I_LEN = bsonArray[0];
            int J_LEN = bsonArray[1];

            PositionInfo[,] array = new PositionInfo[I_LEN, J_LEN];

            for (int x = 2, i = 0; x < bsonArray.Count; x++)
            {
                var arr = bsonArray[x].AsArray;
                int j = 0;
                foreach (BsonValue value in arr)
                {
                    array[i, j] = db.Mapper.Deserialize<PositionInfo>(value);
                    j++;
                }
                i++;
            }

            return array;
        }

        /// <summary>
        /// Lists all robots.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Robot> GetRobots()
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    List<Robot> results = this.robots.Include(r => r.CurrentMap).Query().ToList();
                    return results;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a robot by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Robot GetRobot(int id)
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    return this.robots.Include(r => r.CurrentMap).FindById(new BsonValue(id));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="robot"></param>
        public int InsertRobot(Robot robot)
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    BsonValue val = this.robots.Insert(robot);
                    return val.AsInt32;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="robot"></param>
        public void UpdateRobot(Robot robot)
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    this.robots.Update(robot);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteRobot(int id)
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    this.robots.Delete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lists all maps.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MarsMap> GetMaps()
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    this.maps = db.GetCollection<MarsMap>("maps");
                    List<MarsMap> results = this.maps.Query().ToList();
                    return results;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a map by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MarsMap GetMap(int id)
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    return this.maps.FindById(new BsonValue(id));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void InsertMap(MarsMap map)
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    this.maps.Insert(map);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateMap(MarsMap map)
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    maps.Update(map);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteMap(int id)
        {
            try
            {
                using (var db = new LiteDatabase(dbConnectionString))
                {
                    maps.Delete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
