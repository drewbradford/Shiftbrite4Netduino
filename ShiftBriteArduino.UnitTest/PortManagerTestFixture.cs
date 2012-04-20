using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShiftBriteArduino.FrameworkShims;
using ShiftBriteArduino.IO;

namespace ShiftBriteArduino.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PortManagerTestFixture
    {
        public PortManagerTestFixture()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreatingPortManagerPullsLatchPortLow()
        {

            var latchMock = new Mock<IOutputPort>(MockBehavior.Strict);
            latchMock.Setup(test => test.Write(false));

            var layout = new PortLayout {LatchPort = latchMock.Object};
            var loggerMock = new Mock<ILogger>();

            var portManager = new PortManager(layout, loggerMock.Object);

            latchMock.VerifyAll();
        }

        [TestMethod]
        public void LatchPullsLatchOutputHighAndLow()
        {

            var latchMock = new Mock<IOutputPort>(MockBehavior.Strict);
            latchMock.Setup(test => test.Write(true));
            latchMock.Setup(test => test.Write(false));

            var layout = new PortLayout { LatchPort = latchMock.Object };
            var loggerMock = new Mock<ILogger>();

            var portManager = new PortManager(layout, loggerMock.Object);

            portManager.Latch();
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DifferentlySizedPortAndPacketArraysThrowsException()
        {
            var genericPinMock = new Mock<IOutputPort>();
            var bankZeroMock = new Mock<IOutputPort>();

            var layout = new PortLayout { LatchPort = genericPinMock.Object, ClockPort = genericPinMock.Object, DataPorts = new[] { bankZeroMock.Object } };
            var loggerMock = new Mock<ILogger>();
            var portManager = new PortManager(layout, loggerMock.Object);

            portManager.SendPacketArray(new[] { new CommandPacket(), new CommandPacket(), new CommandPacket() }); 

            
        }
    }
}
