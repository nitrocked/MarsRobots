using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MarsRobots.Entities
{
    public class MarsMap
    {
        PositionInfo[,] mapGrid;

        /// <summary>
        /// Id of the map.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Map name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// X size of map.
        /// </summary>
        public int SizeX { get; private set; }
        /// <summary>
        /// Y size of map.
        /// </summary>
        public int SizeY { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="name"></param>
        public MarsMap(int sizeX, int sizeY, string name = "")
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            mapGrid = new PositionInfo[sizeX, sizeY];
        }

        /// <summary>
        /// Gets the position data for the given coordinate in the map.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public PositionInfo GetPositionInfo(Position pos)
        {
            PositionInfo posInfo = null;
            try
            {
                if (this.IsPointInsideMap(pos))
                {
                    if (this.mapGrid[pos.X, pos.Y] == null)
                        this.mapGrid[pos.X, pos.Y] = new PositionInfo();
                    posInfo = this.mapGrid[pos.X, pos.Y];
                }
            }
            catch(Exception ex)
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
