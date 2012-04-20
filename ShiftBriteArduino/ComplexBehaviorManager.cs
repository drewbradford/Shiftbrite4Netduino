    using System.Threading;

namespace ShiftBriteArduino
{
    public class ComplexBehaviorManager
    {
        private readonly IRgbManager _rgbManager;
        private readonly ILogger _logger;

        public ComplexBehaviorManager(IRgbManager rgbManager, ILogger logger)
        {
            _rgbManager = rgbManager;
            _logger = logger;
        }

        public void RunInitalizeSequence(int delay, int cycles)
        {
            for(int i = 0; i < cycles; i++)
            {
                _rgbManager.FillColor(LedColor.GetRed);
                _rgbManager.PushArrayToDisplay();

                Thread.Sleep(delay);

                _rgbManager.FillColor(LedColor.GetGreen);
                _rgbManager.PushArrayToDisplay();

                Thread.Sleep(delay);

                _rgbManager.FillColor(LedColor.GetBlue);
                _rgbManager.PushArrayToDisplay();

                Thread.Sleep(delay);
            }
        }

        //this could throw an exception if the colors are more than available channels in any bank
        public void ParadeForward(int delay, int cycles, LedColor[] colors)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                _rgbManager.UpdateSingleCrossSection(i, colors[i]);
            }
            _rgbManager.PushArrayToDisplay();

            for(int i = 0; i < cycles; i++)
            {
                Thread.Sleep(delay);
                _rgbManager.ShiftForward(_rgbManager.GetSingleChannel(0, _rgbManager.NumChannels - 1));
                _rgbManager.PushArrayToDisplay();
            }                        
        }

        public void ChaseFadedLight(int bank, int cycles, int delay, LedColor pureColor)
        {           
            _rgbManager.UpdateSingleBank(bank, LedColor.GetNone);
            _rgbManager.PushArrayToDisplay();
                        
            for(int i =0; i < cycles; i++)
            {
                for (int channel = 0; channel < _rgbManager.NumChannels; channel++)
                {
                    WriteFadedLight(bank, channel, pureColor);
                    _rgbManager.PushArrayToDisplay();
                                Thread.Sleep(delay);
                                _logger.LogLight();
                }
                for (int channel = _rgbManager.NumChannels - 1; channel >= 0; channel--)
                {
                    WriteFadedLight(bank, channel, pureColor);
                    _rgbManager.PushArrayToDisplay();
                    Thread.Sleep(delay);
                }
            }
        }

        private void WriteFadedLight(int bank, int channel, LedColor pureColor)
        {
            _rgbManager.UpdateSingleBank(bank, LedColor.GetNone);

            var oneOff = new LedColor(pureColor.Red / 2, pureColor.Green / 2, pureColor.Blue / 2);
            var twoOff = new LedColor(oneOff.Red / 2, oneOff.Green / 2, oneOff.Blue / 2);

            _rgbManager.UpdateSingleChannel(bank, channel, pureColor);

            if (channel > 0)
            {                
                _rgbManager.UpdateSingleChannel(bank, channel - 1, oneOff);
            }
            if (channel < _rgbManager.NumChannels - 1)
            {
                _rgbManager.UpdateSingleChannel(bank, channel + 1, oneOff);
            }

            if (channel > 1)
            {
                _rgbManager.UpdateSingleChannel(bank, channel - 2, twoOff);
            }
            if (channel < _rgbManager.NumChannels - 2)
            {
                _rgbManager.UpdateSingleChannel(bank, channel + 2, twoOff);
            }
        }

        private void FadeOutAll(int delay)
        {
            //calculate the brightest light in the array

        }

    }
}
