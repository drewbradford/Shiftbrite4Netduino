using System;
using ShiftBriteArduino.FrameworkShims;

namespace ShiftBriteArduino.IO
{
    public class PortLayout : IDisposable
    {        
        public IOutputPort[] DataPorts { get; set; }
        public IOutputPort LatchPort { get; set; }
        public IOutputPort ClockPort { get; set; }
        
        public void Dispose()
        {
            foreach (IOutputPort t in DataPorts)
            {
                if(t != null) t.Dispose();
            }
            if (LatchPort != null) LatchPort.Dispose();
            if (ClockPort != null) ClockPort.Dispose();
        }
    }
}
