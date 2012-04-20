using System;

namespace ShiftBriteArduino.FrameworkShims
{
    public interface IOutputPort  : IDisposable
    {
        void Write(bool state);
    }
}
