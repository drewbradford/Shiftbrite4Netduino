using System;
using System.Threading;
using ShiftBriteArduino.FrameworkShims;

namespace ShiftBriteArduino.IO
{
    public interface IPortManager
    {
        void SendPacketArray(CommandPacket[] commandPacket);
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
       
        public void Latch()
        {
            Thread.Sleep(1 + _latchDelayOffset);
            _portLayout.LatchPort.Write(true);
            Thread.Sleep(1 + _latchDelayOffset);
            _portLayout.LatchPort.Write(false);
        }

        /// <summary>
        /// Shifts out an array of packets in parallel, one packet for each pin.
        /// </summary>
        /// <param name="commandPackets">An array of packets to be shifted out in parallel on the data pins.</param>
        public void SendPacketArray(CommandPacket[] commandPackets)
        {
            if(commandPackets.Length != _portLayout.DataPorts.Length)
            {
                throw new ArgumentOutOfRangeException("commandPackets", "The length of the command packet array does not equal the length of the data port array.");
            }

            var values = new uint[commandPackets.Length];
            for(int i = 0; i<commandPackets.Length; i++)
            {
                values[i] = commandPackets[i].GetCommand();
            }
            
            // Lower Clock
            _portLayout.ClockPort.Write(false);

            string s = "";
            for (int i = 0; i < 32; i++)
            {
                
                UInt32 mask = (UInt32)(1 << i);
                bool bWriteVal;
                //for (int portNumber = 0; portNumber < _portLayout.DataPorts.Length; portNumber++)
                //{
                    //bWriteVal = (values[portNumber] & mask) != 0;
                    if ((values[0] & mask) != 0)
                    {
                        s += "1";
                        bWriteVal = true;
                    }
                    else
                    {
                        s += "0";
                        bWriteVal = false;
                    }
                    _portLayout.DataPorts[0].Write(bWriteVal);
                //}
                
             
                // Raise Clock
                _portLayout.ClockPort.Write(true);

                Thread.Sleep(1);

                // Raise Data to prevent IO conflict
                foreach (IOutputPort t in _portLayout.DataPorts)
                {
                    t.Write(true);
                }

                // Lower Clock
                _portLayout.ClockPort.Write(false);
            }
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
