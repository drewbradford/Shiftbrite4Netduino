using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShiftBriteArduino;
using ShiftBriteArduino.IO;
using Environment = ShiftBriteArduino.Environment;

namespace ShiftBriteArduinoTest
{
    [TestClass]
    public class RgbManagerFixture
    {
        [TestMethod]
        public void RedColorFillPopulatesAllRedValues()
        {

            var portManagerMock = new Mock<IPortManager>();
            var rgbManager = new RgbManager(portManagerMock.Object, 4,4);
            rgbManager.FillColor(LedColor.GetRed);

            const string grid = 
                "|R  ||R  ||R  ||R  |" + Environment.NewLine +
                "|R  ||R  ||R  ||R  |" + Environment.NewLine +
                "|R  ||R  ||R  ||R  |" + Environment.NewLine +
                "|R  ||R  ||R  ||R  |" + Environment.NewLine;

            Assert.AreEqual(grid, rgbManager.PrintArray());
        }


        [TestMethod]
        public void WhiteColorFillPopulatesAllValues()
        {

            var portManagerMock = new Mock<IPortManager>();
            var rgbManager = new RgbManager(portManagerMock.Object, 4, 4);
            rgbManager.FillColor(LedColor.GetWhite);

            const string grid =
                "|RGB||RGB||RGB||RGB|" + Environment.NewLine +
                "|RGB||RGB||RGB||RGB|" + Environment.NewLine +
                "|RGB||RGB||RGB||RGB|" + Environment.NewLine +
                "|RGB||RGB||RGB||RGB|" + Environment.NewLine;

            Assert.AreEqual(grid, rgbManager.PrintArray());
        }


        [TestMethod]
        public void UpdateBankUpdatesOneBank()
        {

            var portManagerMock = new Mock<IPortManager>();
            var rgbManager = new RgbManager(portManagerMock.Object, 3, 6);
            rgbManager.UpdateSingleBank(1, LedColor.GetWhite);

            const string grid =
                "|   ||   ||   ||   ||   ||   |" + Environment.NewLine +
                "|RGB||RGB||RGB||RGB||RGB||RGB|" + Environment.NewLine +
                "|   ||   ||   ||   ||   ||   |" + Environment.NewLine;
             
            Assert.AreEqual(grid, rgbManager.PrintArray());
        }

        [TestMethod]
        public void UpdateSingleChannelUpdatesCorrectChannel()
        {

            var portManagerMock = new Mock<IPortManager>();
            var rgbManager = new RgbManager(portManagerMock.Object, 5, 5);            
            rgbManager.UpdateSingleChannel(1, 3, LedColor.GetBlue);
            rgbManager.UpdateSingleChannel(4, 4, LedColor.GetGreen);
            rgbManager.UpdateSingleChannel(0, 4, LedColor.GetRed);
            rgbManager.UpdateSingleChannel(0, 0, LedColor.GetWhite);

            const string grid =
                "|RGB||   ||   ||   ||R  |" + Environment.NewLine +
                "|   ||   ||   ||  B||   |" + Environment.NewLine +
                "|   ||   ||   ||   ||   |" + Environment.NewLine +
                "|   ||   ||   ||   ||   |" + Environment.NewLine +
                "|   ||   ||   ||   || G |" + Environment.NewLine;
              
            Assert.AreEqual(grid, rgbManager.PrintArray());
        }
    }
}
