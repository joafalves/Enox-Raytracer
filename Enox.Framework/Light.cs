using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Light
    {
        #region fields

        private Vector3 position;
        private float r, g, b;

        #endregion

        #region properties

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
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
