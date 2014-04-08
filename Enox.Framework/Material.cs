using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    [Serializable]
    public class Material
    {
        #region fields

        private Color color;
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

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        #endregion

        #region methods

        public static Material FromString(string content)
        {
            var lines = content.Trim().Split('\n');

            var split = lines[0].Split(' ');
            float red = (float)Convert.ToDecimal(split[0]);
            float green = (float)Convert.ToDecimal(split[1]);
            float blue = (float)Convert.ToDecimal(split[2]);

            split = lines[1].Split(' ');
            float ambient = (float)Convert.ToDecimal(split[0]);
            float diffuse = (float)Convert.ToDecimal(split[1]);
            float reflection = (float)Convert.ToDecimal(split[2]);
            float refractionCoef = (float)Convert.ToDecimal(split[3]);
            float refractionIndex = (float)Convert.ToDecimal(split[4]);

            return new Material()
            {
                color = new Color(red, green, blue),
                ambient = ambient,
                diffuse = diffuse,
                reflection = reflection,
                refractionCoef = refractionCoef,
                refractionIndex = refractionIndex
            };
        }

        #endregion
    }
}
