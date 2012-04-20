using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SPOT.Emulator;
using Microsoft.SPOT.Emulator.Gpio;

namespace ShiftBriteEmulator
{
    public partial class Form1 : Form
    {
        private Emulator _emulator;

        GpioPort _motorUpButtonPort, _motorDownButtonPort, _dataPortClock, _dataPortLatch, _dataPortBankZero, _dataPortBankOne;

        public Form1(Emulator emulator)
        {
            _emulator = emulator;

            _motorUpButtonPort = _emulator.FindComponentById("MotorUpButton") as GpioPort;
            _motorDownButtonPort = _emulator.FindComponentById("MotorDownButton") as GpioPort;

            _dataPortClock = _emulator.FindComponentById("DataPortClock") as GpioPort;
            _dataPortLatch = _emulator.FindComponentById("DataPortLatch") as GpioPort;
            _dataPortBankZero = _emulator.FindComponentById("DataPortBankZero") as GpioPort;
            _dataPortBankOne = _emulator.FindComponentById("DataPortBankOne") as GpioPort;
            

            InitializeComponent();

            _dataPortClock.OnGpioActivity +=new GpioActivity(_dataPortClock_OnGpioActivity);
            _dataPortLatch.OnGpioActivity +=new GpioActivity(_dataPortLatch_OnGpioActivity);
            _dataPortBankZero.OnGpioActivity += new GpioActivity(_dataPortBankZero_OnGpioActivity);
            _dataPortBankOne.OnGpioActivity += new GpioActivity(_dataPortBankOne_OnGpioActivity);
            
        }

        void  _dataPortClock_OnGpioActivity(GpioPort sender, bool edge)
        {
 	        throw new NotImplementedException();
        }

        void  _dataPortLatch_OnGpioActivity(GpioPort sender, bool edge)
        {
 	        throw new NotImplementedException();
        }


        void _dataPortBankZero_OnGpioActivity(GpioPort sender, bool edge)
        {
            throw new NotImplementedException();
        }

        void _dataPortBankOne_OnGpioActivity(GpioPort sender, bool edge)
        {
            throw new NotImplementedException();
        }


        delegate void GpioPortWriteDelegate(bool state);

        private void GpioPortSafeWrite(GpioPort port, bool value)
        {
            port.Invoke(new GpioPortWriteDelegate(port.Write), value);
        }

        private void motorUpButton_MouseDown(object sender, MouseEventArgs e)
        {
            GpioPortSafeWrite(_motorUpButtonPort, true);
        }

        private void motorUpButton_MouseUp(object sender, MouseEventArgs e)
        {
            GpioPortSafeWrite(_motorUpButtonPort, false);
        }

        private void motorDownButton_MouseDown(object sender, MouseEventArgs e)
        {
            GpioPortSafeWrite(_motorDownButtonPort, true);
        }

        private void motorDownButton_MouseUp(object sender, MouseEventArgs e)
        {
            GpioPortSafeWrite(_motorDownButtonPort, false);
        }
    }
}
