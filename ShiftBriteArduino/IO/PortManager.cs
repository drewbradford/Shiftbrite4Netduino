using System;
using System.Threading;
using ShiftBriteArduino.FrameworkShims;


namespace ShiftBriteArduino.IO
{
    public interface IPortManager
    {
        void SendPacketArray(CommandPacket[] commandPacket);
        void Latch();
        void wait(UInt64 uSec);
        void setBanks(UInt16 iNumBanks);
    }

    public class PortManager : IPortManager
    {
        private readonly PortLayout _portLayout;
        private readonly ILogger _logger;
        private readonly int _latchDelayOffset;
        public UInt16 _iNumBanks;


        public PortManager(PortLayout portLayout, ILogger logger) : this(portLayout, logger, 0)
        {

        }

        public PortManager(PortLayout portLayout, ILogger logger, int latchDelayOffset)
        {
            _portLayout = portLayout;
            _logger = logger;
            _latchDelayOffset = latchDelayOffset;
            _iNumBanks = 0;

            Initalize();
        }

        private void Initalize() //not sure if we need this or not
        {
            _portLayout.LatchPort.Write(false);
        }

        public void setBanks(UInt16 iNumBanks)
        {
            _iNumBanks = iNumBanks;
        }
       
        public void Latch()
        {
            //Thread.Sleep(1 + _latchDelayOffset);
            wait(1);
            _portLayout.LatchPort.Write(true);
            wait(1);
            //Thread.Sleep(1 + _latchDelayOffset);
            _portLayout.LatchPort.Write(false);
        }

        public const UInt64 ticks_per_millisecond = System.TimeSpan.TicksPerMillisecond;
        public void wait(UInt64 uSec)
        {
            //UInt64 iTicksPerMS = System.TimeSpan.TicksPerMillisecond;
            //UInt64 iStartTick;
            //UInt64 iCurrentTick;
            //UInt64 iNextTick;
            //iStartTick = Microsoft.SPOT.Hardware.Utility.GetMachineTime().Ticks;
            // This is my ghetto ass wait, because I don't feel like getting the above code to work
            UInt64 iUpperLim = uSec;
            //UInt64 iUpperLim = ui16uSec * 1;
            for (UInt64 i = 0; i<iUpperLim; i++);

        }

        /// <summary>
        /// Shifts out an array of packets in parallel, one packet for each pin.
        /// </summary>
        /// <param name="commandPackets">An array of packets to be shifted out in parallel on the data pins.</param>
        public void SendPacketArray(CommandPacket[] commandPackets)
        {
            // DDL, this is only ever 1
            //if(commandPackets.Length != _portLayout.DataPorts.Length)
            //{
            //    throw new ArgumentOutOfRangeException("commandPackets", "The length of the command packet array does not equal the length of the data port array.");
            //}

            var values = new uint[commandPackets.Length];
            for(int i = 0; i<commandPackets.Length; i++)
            {
                values[i] = commandPackets[i].GetCommand();
            }
            
            // Lower Clock
            _portLayout.ClockPort.Write(false);

            string s = "";
            UInt32 mask = 0x80000000;
            for (int i = 0; i < 32; i++)
            {
                
                //UInt32 mask = (UInt32)(1 << i);
                bool bWriteVal;
                
                //for (int portNumber = 0; portNumber < _portLayout.DataPorts.Length; portNumber++)
                for (int portNumber = 0; portNumber < _iNumBanks; portNumber++)
                {
                    bWriteVal = (values[portNumber] & mask) != 0;
                   /*
                    if ((values[portNumber] & mask) != 0)
                    {
                        //s += "1";
                        bWriteVal = true;
                    }
                    else
                    {
                        //s += "0";
                        bWriteVal = false;
                    }
                    */
                    _portLayout.DataPorts[portNumber].Write(bWriteVal);
                  
                }
                
             
                // Raise Clock
                _portLayout.ClockPort.Write(true);

                //wait(1);
                //Thread.Sleep(1);

                // set Data to prevent IO conflict
                /* DDL I took this out because it shaved 600 uS off the clock function (750 uS down to 160 uS)
                foreach (IOutputPort t in _portLayout.DataPorts)
                {
                   t.Write(false);
                }
                */
                // Lower Clock
                _portLayout.ClockPort.Write(false);
                mask = mask >> 1; // shift mask over one bit
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
