using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Solid
    {
        #region fields

        //private int materialIndex;
        private List<Triangle> triangles = new List<Triangle>();

        #endregion

        #region properties

        //public int MaterialIndex
        //{
        //    get { return materialIndex; }
        //    set { materialIndex = value; }
        //}

        public List<Triangle> Triangles
        {
            get { return triangles; }
            set { triangles = value; }
        }

        #endregion

        #region methods

        public static Solid FromString(string content)
        {
            var lines = content.Trim().Split('\n');

            int c = 0;
            Triangle t = new Triangle();
            List<Triangle> triangles = new List<Triangle>();
            for (int i = 0; i < lines.Count(); i++)
            {
                if (c > 3)
                {
                    c = 0;

                    Vector3 u = t.Points[1] - t.Points[0];
                    Vector3 v = t.Points[2] - t.Points[0];
                    t.Normal = u * v;
                    t.Normal = Vector3.Normalize(t.Normal);

                    triangles.Add(t);                 
                }

                if (c == 0)
                {
                    t = new Triangle();
                    t.MaterialIndex = (int)Convert.ToDecimal(lines[i]);
                    t.Points = new Vector3[3];
                }
                else
                {
                    var split = lines[i].Split(' ');

                    t.Points[c - 1] = new Vector3()
                    {
                        //X = (float)Convert.ToDecimal(split[0]), // this way won't detected 'E'
                        //Y = (float)Convert.ToDecimal(split[1]),
                        //Z = (float)Convert.ToDecimal(split[2])
                        X = float.Parse(split[0]),
                        Y = float.Parse(split[1]),
                        Z = float.Parse(split[2])
                    };
                }

                c++;
            }

            return new Solid()
            {
                triangles = triangles
            };
        }

        #endregion
    }
}
