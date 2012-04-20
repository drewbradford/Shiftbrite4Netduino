using System;
using ShiftBriteArduino.FrameworkShims;

namespace ShiftBriteArduino
{
    public class PortLayout : IDisposable
    {
        public IOutputPort BankOneDataPort { get; set; }
        public IOutputPort BankTwoDataPort { get; set; }
        public IOutputPort LatchPort { get; set; }
        public IOutputPort ClockPort { get; set; }
        
        public void Dispose()
        {
            if(BankOneDataPort != null) BankOneDataPort.Dispose();
            if (BankTwoDataPort != null) BankTwoDataPort.Dispose();
            if (LatchPort != null) LatchPort.Dispose();
            if (ClockPort != null) ClockPort.Dispose();
        }
    }
}
