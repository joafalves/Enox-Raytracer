using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Ray
    {
        #region field

        private static readonly float EPSILON = 0.01f;

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
            IntersectionResult tr = NearestIntersection(scene, r);

            if (tr.triangle != null)
            {
                //v1:
                //return scene.Materials[tr.MaterialIndex].Color;                
                // v2: 
                Color c = new Color(0, 0, 0, 1);

                // contribuição luz ambiente:
                foreach (var light in scene.Lights)
                {
                    c += light.Color * scene.Materials[tr.triangle.MaterialIndex].Color * 
                        scene.Materials[tr.triangle.MaterialIndex].Ambient;
                }
                                                              
                // contribuição luz difusa 
                foreach (var light in scene.Lights)
                {
                    Vector3 l = light.Position - tr.point;
                    float dist = Vector3.Length(l);
                    l = Vector3.Normalize(l);
                    float cost = tr.triangle.Normal.Dot(l);

                    Ray s = new Ray()
                    {
                        origin = tr.point,
                        direction = l
                    };

                    if (IsExposedToLight(scene, s, dist) && cost > 0)
                        c = c + light.Color * scene.Materials[tr.triangle.MaterialIndex].Color
                            * scene.Materials[tr.triangle.MaterialIndex].Diffuse * cost;
                }

                // reflection, material reflective?
                if (scene.Materials[tr.triangle.MaterialIndex].Reflection > 0.0f)
                {
                    float c1 = -tr.triangle.Normal.Dot(r.direction);
                    Vector3 rl = Vector3.Normalize(r.direction + (tr.triangle.Normal * 2.0f * c1));
                    Ray refl = new Ray()
                    {
                        origin = tr.point,
                        direction = rl
                    };

                    c += (max > 0 ? Ray.Trace(scene, refl, --max) : new Color(0, 0, 0)) * 
                        scene.Materials[tr.triangle.MaterialIndex].Reflection * 
                        scene.Materials[tr.triangle.MaterialIndex].Color;
                }
                else if (scene.Materials[tr.triangle.MaterialIndex].RefractionCoef > 0.0f)
                {
                    Vector3 n1 = new Vector3();
                    double m1, m2;
                    if (RayEnters(r.direction, tr.triangle.Normal))
                    {
                        n1 = tr.triangle.Normal;
                        m1 = 1.0f; // air
                        m2 = scene.Materials[tr.triangle.MaterialIndex].RefractionIndex;
                    }
                    else
                    {
                        n1 = tr.triangle.Normal * -1;
                        m1 = scene.Materials[tr.triangle.MaterialIndex].RefractionIndex;
                        m2 = 1.0f;
                    }

                    double c1 = -(n1).Dot(r.direction);
                    double m = m1 / m2;
                    double c2 = Math.Sqrt(1 - m * m * (1 - c1 * c1));

                    Vector3 rr = Vector3.Normalize((r.direction * (float)m) + n1 * (float)(m * c1 - c2));
                    Ray refr = new Ray()
                    {
                        origin = tr.point,
                        direction = rr
                    };

                    c += (max > 0 ? Ray.Trace(scene, refr, --max) : new Color(0, 0, 0)) *
                         scene.Materials[tr.triangle.MaterialIndex].RefractionCoef  * 
                         scene.Materials[tr.triangle.MaterialIndex].Color;
                }

                return c;               
            }

            return scene.Image.Color;
        }

        private static bool RayEnters(Vector3 rayN, Vector3 triN)
        {
            if (rayN.Dot(triN) < 0) return true;
            return false;
        }

        private static bool IsExposedToLight(Scene scene, Ray r, float dist)
        {
            // if (intersection  => light : triangles) return true; // basta encontrar 1 (mas dentro da largura entre a luz e o ponto)
            foreach (Solid s in scene.Solids)
            {
                foreach (Triangle tr in s.Triangles)
                {
                    float t = (float)Intersection(r, tr);

                    if (t > 0 && t < dist) return false;
                }
            }           

            return true;
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

            if (detT < EPSILON) return -1;

            return detT;
        }

        private static IntersectionResult NearestIntersection(Scene scene, Ray r)
        {
            Vector3 intersectionPoint = new Vector3(0, 0, 0);
            double tmin = double.MaxValue;
            Triangle nearestTriangle = null;
            foreach (Solid s in scene.Solids)
            {
                foreach (Triangle tr in s.Triangles)
                {
                    float t = (float)Intersection(r, tr);
                    if (t > 0 && t < tmin)
                    {
                        tmin = t;
                        nearestTriangle = tr;
                        intersectionPoint = r.origin + (r.direction * t); // p(t)
                    }
                }
            }

            return new IntersectionResult() {
                triangle = nearestTriangle,
                point = intersectionPoint
            };
        }

        #endregion

        struct IntersectionResult
        {
            internal Triangle triangle;
            internal Vector3 point;
        }
    }
}
