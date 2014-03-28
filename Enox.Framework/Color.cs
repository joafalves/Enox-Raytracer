using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Color
    {
        #region fields

        private float r, g, b, a;

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

        public float A
        {
            get { return a; }
            set { a = value; }
        }

        #endregion

        #region constructors

        public Color()
        {
            this.r = 0;
            this.g = 0;
            this.b = 0;
            this.a = 1;
        }

        public Color(float r, float g, float b, float a = 1)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public override string ToString()
        {
            return string.Format("[r:{0},g:{1},b:{2},a:{3}]", r, g, b, a);
        }

        public static Color operator +(Color a, Color b)
        {
            return new Color(a.R + b.R, a.G + b.G, a.B + b.B);
        }

        public static Color operator *(Color a, Color b)
        {
            return new Color(a.R * b.R, a.G * b.G, a.B * b.B);
        }

        public static Color operator *(Color a, float b)
        {
            return new Color(a.R * b, a.G * b, a.B * b);
        }

        #endregion
    }
}
