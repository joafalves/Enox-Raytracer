using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Image
    {
        #region field

        private int horizontal, vertical;
        private Color color;

        #endregion

        #region properties

        public Color Color
        {
            get { return color; }
            set { color = value;  }
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
            this.color = new Color(r, g, b);
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
                color = new Color(red, green, blue)
            };
        }

        #endregion
    }
}
