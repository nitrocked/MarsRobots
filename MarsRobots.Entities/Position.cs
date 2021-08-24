using MarsRobots.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MarsRobots.Entities
{
    public class Position
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Cardinal direction.
        /// </summary>
        public CardinalDirection direction { get; set; }

        /// <summary>
        /// Default contructor.
        /// </summary>
        public Position()
        {
            this.X = this.Y = 0;
            this.direction = CardinalDirection.N;
        }

        /// <summary>
        /// Contructor with parameters.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        public Position(int x, int y, CardinalDirection direction)
        {
            this.X = x;
            this.Y = y;
            this.direction = direction;
        }

        /// <summary>
        /// Deep copy of current object.
        /// </summary>
        /// <returns></returns>
        public Position Copy()
        {
            return new Position(this.X, this.Y, this.direction);
        }

        /// <summary>
        /// Rotates Cardinal diretion 90º to the left.
        /// </summary>
        /// <param name="pos"></param>
        public void Left()
        {
            this.direction = this.direction.Left();
        }

        /// <summary>
        /// Rotates Cardinal diretion 90º to the right.
        /// </summary>
        /// <param name="pos"></param>
        public void Right()
        {
            this.direction = this.direction.Right();
        }

        /// <summary>
        /// Advances one step towards Cardinal direction.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool Forward()
        {
            bool res = false;
            try
            {
                switch (this.direction)
                {
                    case CardinalDirection.N:
                        this.Y++;
                        break;
                    case CardinalDirection.E:
                        this.X++;
                        break;
                    case CardinalDirection.S:
                        this.Y--;
                        break;
                    case CardinalDirection.W:
                        this.X--;
                        break;
                    default:
                        break;
                }
                res = true;
            }
            catch (Exception ex)
            {
                //TODO implement logging.
                res = false;
                throw ex;
            }

            return res;
        }
    }
}
