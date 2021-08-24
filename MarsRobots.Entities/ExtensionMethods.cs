using MarsRobots.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MarsRobots.Entities
{
    public static class ExtensionMethods
    {



        /// <summary>
        /// Rotates Cardinal diretion 90º to the right.
        /// </summary>
        /// <typeparam name="CardinalDirection"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static CardinalDirection Right<CardinalDirection>(this CardinalDirection src)
        {
            CardinalDirection[] Arr = (CardinalDirection[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<CardinalDirection>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }

        /// <summary>
        /// Rotates Cardinal diretion 90º to the left.
        /// </summary>
        /// <typeparam name="CardinalDirection"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static CardinalDirection Left<CardinalDirection>(this CardinalDirection src) where CardinalDirection : struct
        {
            CardinalDirection[] Arr = (CardinalDirection[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<CardinalDirection>(Arr, src) - 1;
            return (j < 0) ? Arr[Arr.Length - 1] : Arr[j];
        }
    }
}
