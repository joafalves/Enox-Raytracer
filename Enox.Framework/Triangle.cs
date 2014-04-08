using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    [Serializable]
    public class Triangle
    {
        #region fields

        private int materialIndex;
        private Vector3 normal;
        private Vector3[] points;
        
        #endregion

        #region properties

        public int MaterialIndex
        {
            get { return materialIndex; }
            set { materialIndex = value; }
        }

        public Vector3[] Points
        {
            get { return points; }
            set { points = value; }
        }

        public Vector3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }  

        #endregion
    }
}
