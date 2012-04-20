using System;

namespace ShiftBriteArduino
{
    public interface ISerialManager
    {
        void SendSerial(string data);
    }

    public class SerialManager : ISerialManager
    {
        public string ReadSerial()
        {
            return String.Empty;
        }

        public void SendSerial(string data)
        {
            
        }
    }
}
