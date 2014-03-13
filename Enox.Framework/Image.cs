using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Image
    {
        #region field

        private int horizontal, vertical;
        private float r, g, b;

        #endregion

        #region properties

        public float R
        {
            get { return r; }
            set { r = value; }
        }

        public float G
        {
            get { return g; }
            set { g = value; }
        }

        public float B
        {
            get { return b; }
            set { b = value; }
        }

        public int Horizontal
        {
            get { return horizontal; }
            set { horizontal = value; }
        }

        public int Vertical
        {
            get { return vertical; }
            set { vertical = value; }
        }

        #endregion

        #region constructors

        public Image() { }

        public Image(int horizontal, int vertical, float r, float g, float b)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        #endregion

        #region methods

        public static Image FromString(string content)
        {
            var lines = content.Trim().Split('\n');


            float horizontal = (float)Convert.ToDecimal(lines[0].Split(' ')[0]);
            float vertical = (float)Convert.ToDecimal(lines[0].Split(' ')[0]);
            
            var split = lines[1].Split(' ');
            float red = (float)Convert.ToDecimal(split[0]);
            float green = (float)Convert.ToDecimal(split[1]);
            float blue = (float)Convert.ToDecimal(split[2]);

            return new Image()
            {
                horizontal = (int)horizontal,
                vertical = (int)vertical,
                r = red,
                g = green,
                b = blue
            };
        }

        #endregion
    }
}
