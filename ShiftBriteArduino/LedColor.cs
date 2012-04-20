using System;

namespace ShiftBriteArduino
{
    //might consider including intensity on here as well ... not sure if we really need that though
    public class LedColor
    {
        public const int MAX_BRIGHT = 1023;

        private int _red;
        private int _green;
        private int _blue;

        public LedColor(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public int Red
        {
            get { return _red; }
            set 
            { 
                ValidateColorValue(value);
                _red = value;
            }
        }

        public int Green
        {
            get { return _green; }
            set
            {
                ValidateColorValue(value);
                _green = value;
            }
        }

        public int Blue
        {
            get { return _blue; }
            set
            {
                ValidateColorValue(value);
                _blue = value;
            }
        }
        
        private void ValidateColorValue(int color)
        {
            if ((color > 1023) | (color < 0))
            {
                throw new Exception("invalid color value");
            }
        }


        public static LedColor GetNone
        {
            get
            {
                return new LedColor(0, 0, 0);
            }
        }

        public static LedColor GetRed
        {
            get
            {
                return new LedColor(MAX_BRIGHT, 0, 0);    
            }            
        }

        public static LedColor GetGreen
        {
            get
            {
                return new LedColor(0, MAX_BRIGHT, 0);
            }
        }

        public static LedColor GetBlue
        {
            get
            {
                return new LedColor(0, 0, MAX_BRIGHT);
            }
        }

        public static LedColor GetWhite
        {
            get
            {
                return new LedColor(MAX_BRIGHT, MAX_BRIGHT, MAX_BRIGHT);
            }
        }

        public static LedColor GetA
        {
            get
            {
                return new LedColor(MAX_BRIGHT, MAX_BRIGHT, 0);
            }
        }

        public static LedColor GetB
        {
            get
            {
                return new LedColor(0, MAX_BRIGHT, MAX_BRIGHT);
            }
        }

        public static LedColor GetC
        {
            get
            {
                return new LedColor(MAX_BRIGHT, 0, MAX_BRIGHT);
            }
        }

    }
}
