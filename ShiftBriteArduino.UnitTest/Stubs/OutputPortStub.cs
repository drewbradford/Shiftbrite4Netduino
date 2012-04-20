using ShiftBriteArduino.FrameworkShims;

namespace ShiftBriteArduino.UnitTest.Stubs
{

    public class OutputPortStub : IOutputPort
    {
        public void Write(bool state)
        {
            
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
