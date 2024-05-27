using System;
using System.Drawing;

namespace K8055Velleman
{
    public struct Vector2
    {

        public float x = 0, y = 0;

        public readonly float magnitude => (float)Math.Sqrt(x * x + y * y);
        public readonly float sqrMagnitude => x * x + y * y;

        public Vector2() { }

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(Vector2 vector2)
        {
            this.x = vector2.x;
            this.y = vector2.y;
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            return (float)Math.Sqrt(num * num + num2 * num2);
        }

        public override readonly string ToString()
        {
            return $"{x}, {y}";
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
            return new((int)Math.Round(vector2.x), (int)Math.Round(vector2.y));
        }

        public static implicit operator Size(Vector2 vector2)
        {
            return new((int)Math.Round(vector2.x), (int)Math.Round(vector2.y));
        }

    }
}
