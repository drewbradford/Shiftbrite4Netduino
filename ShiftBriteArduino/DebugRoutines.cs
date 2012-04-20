using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using ShiftBriteArduino.FrameworkShims;
using ShiftBriteArduino.IO;

namespace ShiftBriteArduino
{
    public class DebugRoutines
    {
        Microsoft.SPOT.Hardware.OutputPort pin0;
        Microsoft.SPOT.Hardware.OutputPort pin1;
        Microsoft.SPOT.Hardware.OutputPort pin2;
        Microsoft.SPOT.Hardware.OutputPort pin3;
        Microsoft.SPOT.Hardware.OutputPort pin4;
        Microsoft.SPOT.Hardware.OutputPort pin5;
        Microsoft.SPOT.Hardware.OutputPort pin6;
        Microsoft.SPOT.Hardware.OutputPort pin7;

        public DebugRoutines()
        {
            pin0 = new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D0, false);
            pin1 = new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D1, false);
            pin2 = new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D2, false);
            pin3 = new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D3, false);
            pin4 = new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D4, false);
            pin5 = new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D5, false);
            pin6 = new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D6, false);
            pin7 = new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D7, false);

        }

        public void Loop()
        {

                
            while (true)
            {

                pin0.Write(true);
                pin1.Write(true);
                pin2.Write(true);
                pin3.Write(true);
                pin4.Write(true);
                pin5.Write(true);
                pin6.Write(true);
                pin7.Write(true);

                Thread.Sleep(1000);

                pin0.Write(false);
                pin1.Write(false);
                pin2.Write(false);
                pin3.Write(false);
                pin4.Write(false);
                pin5.Write(false);
                pin6.Write(false);
                pin7.Write(false);

                Thread.Sleep(1000);
            }

            
        }
    }
}
