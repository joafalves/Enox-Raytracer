//float d = Vector3.Dot(tr.Normal, tr.Points[0]);
//return (float)-(Vector3.Dot(tr.Normal, r.origin) + d) / Vector3.Dot(tr.Normal, r.direction);

//Triangle a = new Triangle();
//a.Points[0] = new Vector3(tr.Points[0].X - tr.Points[1].X, tr.Points[0].X - tr.Points[2].X, r.direction.X);
//a.Points[1] = new Vector3(tr.Points[0].Y - tr.Points[1].Y, tr.Points[0].Y - tr.Points[2].Y, r.direction.Y);
//a.Points[2] = new Vector3(tr.Points[0].Z - tr.Points[1].Z, tr.Points[0].Z - tr.Points[2].Z, r.direction.Z);

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

        public static Color Trace(Scene scene, Ray r, int max)
        {
            Triangle tr = NearestIntersection(scene, r);

            if (tr != null)
            {
                //v1:
                //return scene.Materials[tr.MaterialIndex].Color;

                
                // v2: 
                Color c = new Color(0, 0, 0, 1);

                // contribuição luz ambiente:
                foreach (var light in scene.Lights)
                {
                    c += light.Color * scene.Materials[tr.MaterialIndex].Color * scene.Materials[tr.MaterialIndex].Ambient;
                }

                return c;

                /*
                
                // contribuição luz difusa
                foreach (var light in scene.Lights)
                {
                    //Vector3 l = normalize(light.Position - P);
                    //float cost = produto_escalar(N) . l; 
                    //if( !IsExposedToLight(light) && cost > 0) c = c + light.Color * scene.Materials[tr.MaterialIndex].Color * scene.Materials[tr.MaterialIndex].Diffuse * cos(º);
                }
                */
            }

            return scene.Images[0].Color; // TODO: Bg color
        }

        private bool IsExposedToLight(Light light)
        {
            // if (intersection  => light : triangles) return true; // basta encontrar 1 (mas dentro da largura entre a luz e o ponto)

            return false;
        }

        private static double MatrixDeterminant(double[,] a, int n)
        {
            int i, j, j1, j2;
            double det = 0;
            double[,] m = null;

            if (n < 1)
            {
                return 0; // error
            }
            else if (n == 1)
            {
                det = a[0, 0];
            }
            else if (n == 2)
            {
                det = a[0, 0] * a[1, 1] * a[1, 0] * a[0, 1];
            }
            else
            {
                det = 0;
                for (j1 = 0; j1 < n; j1++)
                {
                    m = new double[n - 1, n - 1];
                    for (i = 1; i < n; i++)
                    {
                        j2 = 0;
                        for (j = 0; j < n; j++)
                        {
                            if (j == j1) continue;
                            m[i - 1, j2] = a[i, j];
                            j2++;
                        }
                    }
                    det = Math.Pow(-1.0, 1.0 + j1 + 1.0) * a[0, j1] * MatrixDeterminant(m, n - 1);
                }
            }

            return det;
        }

        private static double Determinant(double[,] a)
        {
            return (a[0, 0] * a[1, 1] * a[2, 2] + a[0, 1] * a[1, 2] * a[2, 0] + a[0, 2] * a[1, 0] * a[2, 1]) - (a[0, 2] * a[1, 1] * a[2, 0] + a[0, 0] * a[1, 2] * a[2, 1] + a[1, 0] * a[0, 1] * a[2, 2]);
        }

        private static double Intersection(Ray r, Triangle tr)
        {
            double[,] a = new double[3, 3] {
                {tr.Points[0].X - tr.Points[1].X, tr.Points[0].X - tr.Points[2].X, r.direction.X},
                {tr.Points[0].Y - tr.Points[1].Y, tr.Points[0].Y - tr.Points[2].Y, r.direction.Y},
                {tr.Points[0].Z - tr.Points[1].Z, tr.Points[0].Z - tr.Points[2].Z, r.direction.Z}};

            //double detA = MatrixDeterminant(a, 3);
            double detA = Determinant(a);

            double[,] b = new double[3, 3] {
                {tr.Points[0].X - r.origin.X, tr.Points[0].X - tr.Points[2].X, r.direction.X},
                {tr.Points[0].Y - r.origin.Y, tr.Points[0].Y - tr.Points[2].Y, r.direction.Y},
                {tr.Points[0].Z - r.origin.Z, tr.Points[0].Z - tr.Points[2].Z, r.direction.Z}};

            double detB = Determinant(b) / detA;

            if (detB < 0) return -1;

            double[,] y = new double[3, 3] {
                {tr.Points[0].X - tr.Points[1].X, tr.Points[0].X - r.origin.X, r.direction.X},
                {tr.Points[0].Y - tr.Points[1].Y, tr.Points[0].Y - r.origin.Y, r.direction.Y},
                {tr.Points[0].Z - tr.Points[1].Z, tr.Points[0].Z - r.origin.Z, r.direction.Z}};

            double detY = Determinant(y) / detA;

            if (detY < 0) return -1;

            if (detB + detY >= 1) return -1;

            double[,] t = new double[3, 3] {
                {tr.Points[0].X - tr.Points[1].X, tr.Points[0].X - tr.Points[2].X, tr.Points[0].X  - r.origin.X},
                {tr.Points[0].Y - tr.Points[1].Y, tr.Points[0].Y - tr.Points[2].Y, tr.Points[0].Y  - r.origin.Y},
                {tr.Points[0].Z - tr.Points[1].Z, tr.Points[0].Z - tr.Points[2].Z, tr.Points[0].Z  - r.origin.Z}};

            double detT = Determinant(t) / detA;

            if (detT < 0.0000000001f) return -1;

            return detT;
        }

        private static Triangle NearestIntersection(Scene scene, Ray r)
        {
            double tmin = double.MaxValue;
            Triangle nearestTriangle = null;
            foreach (Solid s in scene.Solids)
            {
                foreach (Triangle tr in s.Triangles)
                {
                    double t = Intersection(r, tr);
                    if (t > 0 && t < tmin)
                    {
                        tmin = t;
                        nearestTriangle = tr;
                    }
                }
            }

            return nearestTriangle;
        }

        #endregion
    }
}
