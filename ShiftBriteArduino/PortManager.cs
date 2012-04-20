using System;
using System.Threading;
using ShiftBriteArduino.FrameworkShims;

namespace ShiftBriteArduino
{
    public interface IPortManager
    {
        void SendPacket(int bank, CommandPacket commandPacket);
        void Latch();
    }

    public class PortManager : IPortManager
    {
        private readonly PortLayout _portLayout;
        private readonly ILogger _logger;
        private readonly int _latchDelayOffset;


        public PortManager(PortLayout portLayout, ILogger logger) : this(portLayout, logger, 0)
        {

        }

        public PortManager(PortLayout portLayout, ILogger logger, int latchDelayOffset)
        {
            _portLayout = portLayout;
            _logger = logger;
            _latchDelayOffset = latchDelayOffset;

            Initalize();
        }

        private void Initalize() //not sure if we need this or not
        {
            _portLayout.LatchPort.Write(false);
        }

        public void SendPacket(int bank, CommandPacket commandPacket)
        {
            IOutputPort dataPort;
            switch (bank)
            {
                case 0:
                    dataPort = _portLayout.BankOneDataPort;
                    break;
                case 1:
                    dataPort = _portLayout.BankTwoDataPort;
                    break;
                default:
                    throw new Exception("invalid bank");
            }

            ulong command = commandPacket.GetCommand();

            ShiftOut(dataPort, _portLayout.ClockPort, BitOrder.MSBFirst, ((byte)(command >> 24)));
            ShiftOut(dataPort, _portLayout.ClockPort, BitOrder.MSBFirst, ((byte)(command >> 16)));
            ShiftOut(dataPort, _portLayout.ClockPort, BitOrder.MSBFirst, ((byte)(command >> 8)));
            ShiftOut(dataPort, _portLayout.ClockPort, BitOrder.MSBFirst, ((byte)command));

            _logger.Log("Shifted command packet:");
            _logger.Log(command.ToString()); //maybe?
        }


        public void Latch()
        {
            Thread.Sleep(1 + _latchDelayOffset);
            _portLayout.LatchPort.Write(true);
            Thread.Sleep(1 + _latchDelayOffset);
            _portLayout.LatchPort.Write(false);
        }

        private static void ShiftOut(IOutputPort dataPort, IOutputPort clockPort, BitOrder bitOrder,  byte value)
        {
            // Lower Clock
            clockPort.Write(false);

            for (int i = 0; i < 8; i++)
            {
                byte mask;
                if (bitOrder == BitOrder.LSBFirst)
                    mask = (byte)(1 << i);
                else
                    mask = (byte)(1 << (7 - i));

                dataPort.Write((value & mask) != 0);

                // Raise Clock
                clockPort.Write(true);

                // Raise Data to prevent IO conflict
                dataPort.Write(true);

                // Lower Clock
                clockPort.Write(false);
            }
        }
    }
}
