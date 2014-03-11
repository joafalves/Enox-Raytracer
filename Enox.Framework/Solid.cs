using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Solid
    {
        #region fields

        private int materialIndex;
        private List<Vector3> vertices = new List<Vector3>(); 

        #endregion

        #region properties

        public int MaterialIndex
        {
            get { return materialIndex; }
            set { materialIndex = value; }
        }

        public List<Vector3> Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        #endregion
    }
}
