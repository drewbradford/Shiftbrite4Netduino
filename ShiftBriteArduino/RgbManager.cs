using ShiftBriteArduino.IO;

namespace ShiftBriteArduino
{
    public interface IRgbManager
    {
        int NumBanks { get; }
        int NumChannels { get; }
        void ClearArray();
        void FillColor(LedColor color);
        void ShiftForward(LedColor replacementColor);
        void UpdateSingleCrossSection(int channel, LedColor color);
        void UpdateSingleBank(int bank, LedColor color);
        void UpdateSingleChannel(int bank, int channel, LedColor color);
        LedColor GetSingleChannel(int bank, int channel);
        void PushArrayToDisplay();
        string PrintArray();
    }

    public class RgbManager : IRgbManager
    {
        private readonly int[][][] _rgbArray; //bank, channel, rgb values

        private readonly IPortManager _portManager;
        private readonly int _numBanks;
        private readonly int _numChannels;

        //TODO:refactor to allow variable lengths for each chain
        public RgbManager(IPortManager portManager, int numBanks, int numChannels) 
        {
            _portManager = portManager;
            _numBanks = numBanks;
            _numChannels = numChannels;

            _rgbArray = new int[_numBanks][][];

            for (int bank = 0; bank < _rgbArray.Length; bank++)
            {
                _rgbArray[bank] = new int[_numChannels][];
                for (int channel = 0; channel < _rgbArray[bank].Length; channel++)
                {
                    _rgbArray[bank][channel] = new int[3];
                }
            }
        }

        public int NumBanks
        {
            get { return _numBanks; }
        }

        public int NumChannels
        {
            get { return _numChannels; }
        }

        public void ClearArray()
        {
            FillColor(new LedColor(0, 0, 0));
        }

        public void FillColor(LedColor color)
        {
            for (int bank = 0; bank < _numBanks; bank++)
            {
                for (int channel = 0; channel < _numChannels; channel++)
                {
                    UpdateSingleChannel(bank, channel, color);
                }
            }
        }


        public void ShiftForward(LedColor replacementColor)
        {
            foreach (var bank in _rgbArray)
            {
                for (int channelToUpdate = bank.Length; channelToUpdate > 1; channelToUpdate--)
                {
                    bank[channelToUpdate][0] = bank[channelToUpdate][0] - 1;
                    bank[channelToUpdate][1] = bank[channelToUpdate][1] - 1;
                    bank[channelToUpdate][2] = bank[channelToUpdate][2] - 1;
                }

                bank[0][0] = replacementColor.Red;
                bank[0][0] = replacementColor.Green;
                bank[0][0] = replacementColor.Blue;
            }
        }

        public void UpdateSingleCrossSection(int channel, LedColor color)
        {
            for (int bank = 0; bank < _rgbArray.Length; bank++)
            {
                UpdateSingleChannel(bank, channel, color);
            }
        }

        public void UpdateSingleBank(int bank, LedColor color)
        {
            for(int channel = 0; channel < _rgbArray[bank].Length; channel++)
            {
                UpdateSingleChannel(bank, channel, color);
            }
        }

        public void UpdateSingleChannel(int bank, int channel, LedColor color)
        {
            _rgbArray[bank][channel][0] = color.Red;
            _rgbArray[bank][channel][1] = color.Green;
            _rgbArray[bank][channel][2] = color.Blue;
        }

        public LedColor GetSingleChannel(int bank, int channel)
        {
            return new LedColor(_rgbArray[bank][channel][0], _rgbArray[bank][channel][1], _rgbArray[bank][channel][2]);
        }

        public void PushArrayToDisplay()
        {
            //for jagged arrays, we could pad with dummy packets on the end to even out the array into a square

            for (int channel = 0; channel < NumChannels; channel++)
            {
                var colorPackets = new CommandPacket[NumBanks];
                var controlPackets = new CommandPacket[NumBanks];

                for (int bank = 0; bank < NumBanks; bank++)
                {
                    colorPackets[bank] = CreateColorPacket(bank, _rgbArray[bank][channel][0], _rgbArray[bank][channel][1], _rgbArray[bank][channel][2]);
                    controlPackets[bank] = CreateControlPacket();
                }

                _portManager.SendPacketArray(colorPackets);
                _portManager.Latch();
                //_portManager.SendPacketArray(controlPackets);
                //_portManager.Latch();
            }                    
        }

        /// <summary>
        /// Each bank is represented as a horizontal string of RGB values.
        /// Multiple banks span multiple lines.
        /// (x is channel, y is bank.  Top left is 0,0).
        /// </summary>
        /// <returns></returns>
        public string PrintArray()
        {      
            string s = "";
            
            for (int bank = 0; bank < NumBanks; bank++)                
            {
                for (int channel = 0; channel < NumChannels; channel++)
                {
                    s += PrintSingleChannel(bank, channel);
                }
                s += Environment.NewLine;
            }
            
      
            return s;
        }


        public int GetBrightestAverageValue()
        {
            int max = 0;
            foreach (var bank in _rgbArray)
            {
                foreach (var channel in bank)
                {
                    int newVal = (channel[0] + channel[1] + channel[2]) / 3;
                    if (newVal > max)
                    {
                        max = newVal;
                    }
                }
            }
            return max;
        }

        private string PrintSingleChannel(int bank, int channel)
        {            
            var s = "|";
            s += _rgbArray[bank][channel][0] > 0 ? "R" : " ";
            s += _rgbArray[bank][channel][1] > 0 ? "G" : " ";
            s += _rgbArray[bank][channel][2] > 0 ? "B" : " ";
            s += "|";

            return s;
        }

        private CommandPacket CreateControlPacket()
        {
            var packet = new CommandPacket
            {
                CommandMode = 00, //write to voltage (current) control registers
                RedCommand = 127,
                GreenCommand = 127,
                BlueCommand = 127
            };

            return packet;
        }

        private CommandPacket CreateColorPacket(int bank, int r, int g, int b)
        {
            var packet = new CommandPacket
            {
                CommandMode = 00, //write to PWM control registers
                RedCommand = r,
                GreenCommand = g,
                BlueCommand = b
            };

            return packet;
        }       
    }
}
