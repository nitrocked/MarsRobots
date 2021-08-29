using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarsRobots.Business;
using System;
using System.Collections.Generic;
using System.Text;
using MarsRobots.Entities;
using MarsRobots.Entities.Enums;

namespace MarsRobots.Business.Tests
{
    [TestClass()]
    public class MarsRobotsHelperTests
    {
        /// <summary>
        /// Tests a minimum size map creation
        /// </summary>
        [TestMethod()]
        public void CreateNewMapTest()
        {
            int testX = 0;
            int testY = 0;

            MarsMap refMap = new MarsMap(testX, testY);

            MarsRobotsBusiness manager = new MarsRobotsBusiness();
            MarsMap tstMap = manager.CreateMap(testX, testY);

            Assert.IsNotNull(tstMap);
            Assert.IsTrue(refMap.SizeX == tstMap.SizeX);
            Assert.IsTrue(refMap.SizeY == tstMap.SizeY);
        }

        /// <summary>
        /// Test creation of inside limits valid map.
        /// </summary>
        [TestMethod()]
        public void CreateNewMapTest1()
        {
            int testX = 50;
            int testY = 25;

            MarsMap refMap = new MarsMap(testX, testY);

            MarsRobotsBusiness manager = new MarsRobotsBusiness();
            MarsMap tstMap = manager.CreateMap(testX, testY);

            Assert.IsNotNull(tstMap);
            Assert.IsTrue(refMap.SizeX == tstMap.SizeX);
            Assert.IsTrue(refMap.SizeY == tstMap.SizeY);
        }

        /// <summary>
        /// Tests creation of map out of business maximum coordinates limit.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateNewMapTest2()
        {
            int testX = 51;
            int testY = 51;

            MarsMap refMap = new MarsMap(testX, testY);
            MarsRobotsBusiness manager = new MarsRobotsBusiness();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => manager.CreateMap(testX, testY));
        }

        [TestMethod()]
        public void CreateRobotTest()
        {
            int testX = 0;
            int testY = 0;
            CardinalDirection dir = CardinalDirection.S;
            string actions = "FFFFFFFFFF";
            MarsRobotsBusiness manager = new MarsRobotsBusiness();
            Robot r1 = manager.CreateRobot(testX, testY, dir, new List<string>() { actions });
            Robot r2 = manager.GetRobot(r1.Id);

            Assert.AreEqual(r1, r2);
        }

        [TestMethod()]
        public void RunRobotTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadMapConfigTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadRobotConfigTest()
        {
            Assert.Fail();
        }
    }
}