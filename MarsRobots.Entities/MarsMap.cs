using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using MarsRobots.Entities.Enums;
using System.Linq;
using System.Text.Json.Serialization;

namespace MarsRobots.Entities
{
    public class MarsMap
    {
        public PositionInfo[,] grid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 

        //public Dictionary<int, Dictionary<int, PositionInfo>> mapGrid { get; set; }

        /// <summary>
        /// Id of the map.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// X size of map.
        /// </summary>
        public int SizeX { get; set; }

        /// <summary>
        /// Y size of map.
        /// </summary>
        public int SizeY { get; set; }

        public MarsMap()
        {
            //mapGrid = new Dictionary<int, Dictionary<int, PositionInfo>>();
            grid = new PositionInfo[,] { };
        }

        /// <summary>
        /// Constructor. Zero based.
        /// </summary>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        /// <param name="name"></param>
        public MarsMap(int maxX, int maxY)
        {
            this.SizeX = maxX + 1;
            this.SizeY = maxY + 1;
            //mapGrid = new Dictionary<int, Dictionary<int, PositionInfo>>();
            grid = new PositionInfo[SizeX, SizeY];
        }

        /// <summary>
        /// Gets the position data for the given coordinate in the map.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        //public PositionInfo GetPositionInfo(Position pos)
        //{
        //    PositionInfo posInfo = null;
        //    try
        //    {
        //        if (this.IsPointInsideMap(pos))
        //        {
        //            if (!this.mapGrid.ContainsKey(pos.X))
        //            {
        //                Dictionary<int, PositionInfo> yComp = new Dictionary<int, PositionInfo>();
        //                yComp.Add(pos.Y, new PositionInfo(pos));
        //                this.mapGrid.Add(pos.X, yComp);
        //            }
        //            else if (!mapGrid[pos.X].ContainsKey(pos.Y))
        //            {
        //                mapGrid[pos.X].Add(pos.Y, new PositionInfo(pos));
        //            }

        //            return mapGrid[pos.X][pos.Y];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return posInfo;
        //}

        public PositionInfo GetPositionInfo(Position pos)
        {
            PositionInfo posInfo = null;
            try
            {
                if (this.IsPointInsideMap(pos))
                {
                    if (this.grid[pos.X, pos.Y] == null)
                        this.grid[pos.X, pos.Y] = new PositionInfo(pos);
                    posInfo = this.grid[pos.X, pos.Y];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return posInfo;
        }

        /// <summary>
        /// Determines if the position is inside the map bounds.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsPointInsideMap(Position pos)
        {
            return (pos != null && pos.X >= 0 && pos.X < this.SizeX
                    && pos.Y >= 0 && pos.Y < this.SizeY);
        }
    }
}
