using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Vector3
    {
        #region fields

        private static Vector3 zero = new Vector3(0f, 0f, 0f);
        public float X, Y, Z;

        #endregion

        #region constructors

        public Vector3() { }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        #region methods

        public float Dot(Vector3 b)
        {
            return b.X * X + b.Y * Y + b.Z * Z;
        }

        public Vector3 Cross(Vector3 b)
        {
            return new Vector3()
            {
                X = Y * b.Z - Z * b.Y,
                Y = Z * b.X - X * b.Z,
                Z = X * b.Y - Y * b.X
            };
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", X, Y, Z);
        }

        public static Vector3 Normalize(Vector3 vector)
        {
            float l = Length(vector);
            return new Vector3()
            {
                X = vector.X / l,
                Y = vector.Y / l,
                Z = vector.Z / l
            };
        }

        public static float Length(Vector3 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
        }

        #endregion

        #region operators 

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        #endregion
    }
}
