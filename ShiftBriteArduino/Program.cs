using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using ShiftBriteArduino.FrameworkShims;
using ShiftBriteArduino.IO;
using OutputPort = ShiftBriteArduino.FrameworkShims.OutputPort;

namespace ShiftBriteArduino
{
    public class Program
    {


        private const int NUM_BANKS = 2;
        private const int NUM_CHANNELS = 2 ;

        public static void Main()
        {
            //setup dependencies -- no container manager

            //DebugRoutines debugRoutines = new DebugRoutines();
            //debugRoutines.Loop();


            IOutputPort bankZeroDataPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D0, false));
            IOutputPort bankOneDataPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D1, false));
            IOutputPort bankTwoDataPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D2, false));
            IOutputPort bankThreeDataPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D3, false));
            IOutputPort bankFourDataPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D4, false));
            IOutputPort bankFiveDataPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D5, false));
            IOutputPort bankSixDataPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D8, false));
            IOutputPort bankSevenDataPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D9, false));
            IOutputPort bankEightDataPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D10, false));

            IOutputPort ledPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.ONBOARD_LED, false));

            var portLayout = new PortLayout
                {                    
                    DataPorts = new[] { bankZeroDataPort,
                                        bankOneDataPort, 
                                        bankTwoDataPort,
                                        bankThreeDataPort,
                                        bankFourDataPort,
                                        bankFiveDataPort,
                                        bankSixDataPort,
                                        bankSevenDataPort,
                                        bankEightDataPort
                                        },
                    ClockPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D6, false)),
                    LatchPort = new OutputPort(new Microsoft.SPOT.Hardware.OutputPort(Pins.GPIO_PIN_D7, false))
                };

            var logger = new Logger(ledPort);
            var portManager = new PortManager(portLayout, logger);
            portManager.setBanks(NUM_BANKS); // DDL, you can move this later
            var rgbManager = new RgbManager(portManager, NUM_BANKS, NUM_CHANNELS);
            var complexBehaviorManager = new ComplexBehaviorManager(rgbManager, logger);


            //begin behavior routines
            complexBehaviorManager.RunInitalizeSequence(1000, 500);


            complexBehaviorManager.ChaseFadedLight(0, 10, 100, LedColor.GetBlue);

            var colors = new LedColor[6];
            colors[0] = LedColor.GetRed;
            colors[1] = LedColor.GetGreen;
            colors[2] = LedColor.GetBlue;
            colors[3] = LedColor.GetA;
            colors[4] = LedColor.GetB;
            colors[5] = LedColor.GetC;
            complexBehaviorManager.ParadeForward(1000, 500, colors);

            
            portLayout.Dispose();

        }


    }
}
