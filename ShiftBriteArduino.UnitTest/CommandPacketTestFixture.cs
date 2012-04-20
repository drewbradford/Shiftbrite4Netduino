using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftBriteArduino;

namespace ShiftBriteArduino.UnitTest
{
    [TestClass]
    public class CommandPacketTestFixture
    {
        [TestMethod]
        public void DefaultConstructorHasAllFieldsZero()
        {
            var packet = new CommandPacket();

            Assert.AreEqual(packet.CommandMode, 0);
            Assert.AreEqual(packet.RedCommand, 0);
            Assert.AreEqual(packet.GreenCommand, 0);
            Assert.AreEqual(packet.BlueCommand, 0);
        }

        [TestMethod]
        public void RedValuePlacedCorrectly()
        {
            var packet = new CommandPacket {RedCommand = 1023};


            var command = packet.GetCommand();

            Assert.AreEqual("11111111110000000000", Convert.ToString(command, 2));
        }

        [TestMethod]
        public void GreenValuePlacedCorrectly()
        {
            var packet = new CommandPacket {GreenCommand = 1023};


            var command = packet.GetCommand();

            Assert.AreEqual("1111111111", Convert.ToString(command, 2));
        }

        [TestMethod]
        public void BlueValuePlacedCorrectly()
        {
            var packet = new CommandPacket {BlueCommand = 1023};


            var command = packet.GetCommand();

            Assert.AreEqual("111111111100000000000000000000", Convert.ToString(command, 2));
        }

        [TestMethod]
        public void CommandValuePlacedCorrectly()
        {
            var packet = new CommandPacket { CommandMode = 1 };


            var command = packet.GetCommand();

            Assert.AreEqual("1000000000000000000000000000000", Convert.ToString(command, 2));
        }
    }
}
