using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Material
    {
        #region fields

        private float r, g, b;
        private float ambient, diffuse, reflection, refractionCoef, refractionIndex;

        #endregion

        #region properties

        public float RefractionIndex
        {
            get { return refractionIndex; }
            set { refractionIndex = value; }
        }

        public float RefractionCoef
        {
            get { return refractionCoef; }
            set { refractionCoef = value; }
        }

        public float Reflection
        {
            get { return reflection; }
            set { reflection = value; }
        }

        public float Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }

        public float Ambient
        {
            get { return ambient; }
            set { ambient = value; }
        }

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

        #endregion
    }
}
