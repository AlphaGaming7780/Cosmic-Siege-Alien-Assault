using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman
{
    public struct Vector2
    {

        public static Vector2 down = new(0, 1);
        public static Vector2 left = new(-1, 0);
        public static Vector2 one = new(-1, -1);
        public static Vector2 under = new(1, 1);
        public static Vector2 right = new(1, 0);
        public static Vector2 up = new(0, -1);
        public static Vector2 zero = new(0, 0);

        public int x = 0, y = 0;

        public readonly float magnitude => (float)Math.Sqrt(x * x + y * y);
        public readonly float sqrMagnitude => x * x + y * y;

        public readonly Vector2 normalized {
            get
            {
                Vector2 vector2 = new(this);
                vector2.Normalize();
                return vector2;
            } 
        }

        public Vector2() { }

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(float x, float y)
        {
            this.x = (int)Math.Round(x);
            this.y = (int)Math.Round(y);
        }

        public Vector2(Vector2 vector2)
        {
            this.x = vector2.x;
            this.y = vector2.y;
        }

        public void Normalize()
        {
            float num = magnitude;
            if (num > 0)
            {
                this /= num;
            }
            else
            {
                this = zero;
            }
        }

        public static Vector2 Scale(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        public void Scale(Vector2 scale)
        {
            x *= scale.x;
            y *= scale.y;
        }

        public static float Dot(Vector2 lhs, Vector2 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }

        public static float Angle(Vector2 from, Vector2 to)
        {
            float num = (float)Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            if (num < 1E-15f)
            {
                return 0f;
            }

            float num2 = Mathf.Clamp(Dot(from, to) / num, -1f, 1f);
            return (float)Math.Acos(num2) * 57.29578f;
        }

        public static float SignedAngle(Vector2 from, Vector2 to)
        {
            float num = Angle(from, to);
            float num2 = Math.Sign(from.x * to.y - from.y * to.x);
            return num * num2;
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            return (float)Math.Sqrt(num * num + num2 * num2);
        }

        public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
        {
            float num = vector.sqrMagnitude;
            if (num > maxLength * maxLength)
            {
                float num2 = (float)Math.Sqrt(num);
                float num3 = vector.x / num2;
                float num4 = vector.y / num2;
                return new Vector2(num3 * maxLength, num4 * maxLength);
            }

            return vector;
        }
        public static Vector2 Min(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
        }

        public static Vector2 Max(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
        }

        public override readonly string ToString()
        {
            return $"{x}, {y}"; // UnityString.Format("({0:F1}, {1:F1})", x, y);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        public override readonly bool Equals(object other)
        {
            if (other is not Vector2)
            {
                return false;
            }

            return Equals((Vector2)other);
        }

        public readonly bool Equals(Vector2 other)
        {
            return x == other.x && y == other.y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator +(Vector2 a, Size b)
        {
            return new Vector2(a.x + b.Width, a.y + b.Height);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator -(Vector2 a, Size b)
        {
            return new Vector2(a.x - b.Width, a.y - b.Height);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(0 - a.x, 0 - a.y);
        }

        public static Vector2 operator *(Vector2 a, int d)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator *(int d, Vector2 a)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator /(Vector2 a, int d)
        {
            return new Vector2(a.x / d, a.y / d);
        }

        public static Vector2 operator /(Vector2 a, float d)
        {
            return new Vector2(a.x / d, a.y / d);
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator Vector2(Size size)
        {
            return new(size.Width, size.Height);
        }

        public static implicit operator Vector2(Point point)
        {
            return new(point.X, point.Y);
        }

        public static implicit operator Point(Vector2 vector2)
        {
            return new(vector2.x, vector2.y);
        }

        public static implicit operator Size(Vector2 vector2)
        {
            return new(vector2.x, vector2.y);
        }

    }
}
