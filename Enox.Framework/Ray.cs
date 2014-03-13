using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Ray
    {
        #region field

        private Vector3 origin;
        private Vector3 direction;       

        #endregion

        #region properties

        public Vector3 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        #endregion

        #region methods

        public static Vector3 Trace(Ray r)
        {
            return new Vector3();
        }

        #endregion
    }
}
