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

        #region methods

        public static Camera FromString(string content)
        {
            var lines = content.Trim().Split('\n');

            float distance = (float)Convert.ToDecimal(lines[0]);
            float fieldOfView = (float)Convert.ToDecimal(lines[1]);
            Vector3 position = new Vector3()
            {
                X = (float)Convert.ToDecimal(lines[2].Split(' ')[0]),
                Y = (float)Convert.ToDecimal(lines[2].Split(' ')[1]),
                Z = (float)Convert.ToDecimal(lines[2].Split(' ')[2])
            };

            float horizontal = 0;
            float vertical = 0;
            if (lines.Count() > 3) // since it's optional, this is important!
            {
                horizontal = (float)Convert.ToDecimal(lines[3].Split(' ')[0]);
                vertical = (float)Convert.ToDecimal(lines[3].Split(' ')[1]);
            }

            return new Camera()
            {
                distance = distance,
                fieldOfView = fieldOfView,
                position = position,
                horizontal = horizontal,
                vertical = vertical
            };
        }

        //public override string ToString()
        //{
        //    return string.Format("Position: {0}\n")
        //}

        #endregion
    }
}
