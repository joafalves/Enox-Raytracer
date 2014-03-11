using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Camera
    {
        #region fields

        private float distance, fieldOfView, horizontal, vertical;
        private Vector3 position;

       

        #endregion

        #region properties

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public float FieldOfView
        {
            get { return fieldOfView; }
            set { fieldOfView = value; }
        }

        public float Horizontal
        {
            get { return horizontal; }
            set { horizontal = value; }
        }

        public float Vertical
        {
            get { return vertical; }
            set { vertical = value; }
        }

        #endregion

        #region constructors

        public static Camera FromString(string content)
        {
            var lines = content.Trim().Split('\n');

            foreach (var line in lines)
            {
                var lineSplit = line.Split(' ');
            }

            return new Camera()
            {


            };
        }

        #endregion
    }
}
