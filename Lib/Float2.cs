using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman
{
    public struct Float2
    {
        public float x = 0;
        public float y = 0;

        public float moyenne { 
            get {
                return (x + y) / 2;
            } 
        }

        public Float2() { }

        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Float2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Float2(Size size)
        {
            return new(size.Width, size.Height);
        }

        public static implicit operator Float2(Point point)
        {
            return new(point.X, point.Y);
        }

        public static explicit operator Point(Float2 float2)
        {
            return new((int)float2.x, (int)float2.y);
        }

        public static explicit operator Float2(Vector2 vector2)
        {
            return new(vector2.x, vector2.y);
        }

        public static Float2 operator /(Float2 a, Float2 b)
        {
            if (b.x == 0 || b.y == 0)
            {
                throw new DivideByZeroException();
            }
            return new Float2(a.x/b.x, a.y/b.y);
        }

    }
}
