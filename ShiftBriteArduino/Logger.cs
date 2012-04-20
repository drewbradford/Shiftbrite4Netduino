using ShiftBriteArduino.FrameworkShims;
using System.Threading;
namespace ShiftBriteArduino
{
    public interface ILogger
    {
        void Log(string message);
        void LogBreak();
        void LogLight();
    }

    public class Logger : ILogger
    {

        private IOutputPort _ledPort;

        public Logger(IOutputPort ledPort)
        {
            _ledPort = ledPort;
            //save a serial port
        }

        private const string Divider = "========";

        private string _message;

        public string Message { get { return _message; } }

        public void Log(string format, params object[] args)
        {
            
        }

        public void Log(string message)
        {
            //_message += message + Divider;
            
            //write to port
        }

        public void LogBreak()
        {
            Log(Divider);
        }

        public void LogLight()
        {
            _ledPort.Write(true);
            Thread.Sleep(100);
            _ledPort.Write(false);
        }
    }
}
