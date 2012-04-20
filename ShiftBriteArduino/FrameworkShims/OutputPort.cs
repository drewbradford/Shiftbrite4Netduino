namespace ShiftBriteArduino.FrameworkShims
{

    public class OutputPort : IOutputPort
    {
        private readonly Microsoft.SPOT.Hardware.OutputPort _internalPort;

        public OutputPort(Microsoft.SPOT.Hardware.OutputPort outputPort)
        {
            _internalPort = outputPort;
        }
        
        public void Write(bool state)
        {
            _internalPort.Write(state);
        }

        public void Dispose()
        {
            _internalPort.Dispose();
        }
    }

}
