using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftBriteArduino;

namespace ShiftBriteArduino
{
    public class CommandPacket
    {
        public int CommandMode { get; set; }
        public int RedCommand { get; set; }
        public int GreenCommand { get; set; }
        public int BlueCommand { get; set; }

        //this is 32 bit value:
        //left two bits: command. next 10: green. next 10: red.  last 10: blue
        //00 0000000000 1111111111 0000000000 would be pure red, for example.
        public UInt32 GetCommand()
        {
            var command = new uint();
            command = (uint)CommandMode & 0xB11;
            command = (command << 10) | ((uint)BlueCommand & 1023);
            command = (command << 10) | ((uint)RedCommand & 1023);
            command = (command << 10) | ((uint)GreenCommand & 1023);

            return command;
        }
    }   
}
