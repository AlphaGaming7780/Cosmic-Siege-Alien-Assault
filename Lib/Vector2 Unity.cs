//using System;

//namespace K8055Velleman;

////
//// Résumé :
////     Representation of 2D vectors and points.
//public struct Vector22 : IEquatable<Vector2>
//{
//    //
//    // Résumé :
//    //     X component of the vector.
//    public float x;

//    //
//    // Résumé :
//    //     Y component of the vector.
//    public float y;

//    private static readonly Vector2 zeroVector = new Vector2(0f, 0f);

//    private static readonly Vector2 oneVector = new Vector2(1f, 1f);

//    private static readonly Vector2 upVector = new Vector2(0f, 1f);

//    private static readonly Vector2 downVector = new Vector2(0f, -1f);

//    private static readonly Vector2 leftVector = new Vector2(-1f, 0f);

//    private static readonly Vector2 rightVector = new Vector2(1f, 0f);

//    private static readonly Vector2 positiveInfinityVector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

//    private static readonly Vector2 negativeInfinityVector = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

//    public const float kEpsilon = 1E-05f;

//    public const float kEpsilonNormalSqrt = 1E-15f;

//    public float this[int index]
//    {
//        get
//        {
//            return index switch
//            {
//                0 => x,
//                1 => y,
//                _ => throw new IndexOutOfRangeException("Invalid Vector2 index!"),
//            };
//        }
//        set
//        {
//            switch (index)
//            {
//                case 0:
//                    x = value;
//                    break;
//                case 1:
//                    y = value;
//                    break;
//                default:
//                    throw new IndexOutOfRangeException("Invalid Vector2 index!");
//            }
//        }
//    }

//    //
//    // Résumé :
//    //     Returns this vector with a magnitude of 1 (Read Only).
//    public Vector2 normalized
//    {
//        get
//        {
//            Vector2 result = new Vector2(x, y);
//            result.Normalize();
//            return result;
//        }
//    }

//    //
//    // Résumé :
//    //     Returns the length of this vector (Read Only).
//    public float magnitude => (float)Math.Sqrt(x * x + y * y);

//    //
//    // Résumé :
//    //     Returns the squared length of this vector (Read Only).
//    public float sqrMagnitude => x * x + y * y;

//    //
//    // Résumé :
//    //     Shorthand for writing Vector2(0, 0).
//    public static Vector2 zero => zeroVector;

//    //
//    // Résumé :
//    //     Shorthand for writing Vector2(1, 1).
//    public static Vector2 one => oneVector;

//    //
//    // Résumé :
//    //     Shorthand for writing Vector2(0, 1).
//    public static Vector2 up => upVector;

//    //
//    // Résumé :
//    //     Shorthand for writing Vector2(0, -1).
//    public static Vector2 down => downVector;

//    //
//    // Résumé :
//    //     Shorthand for writing Vector2(-1, 0).
//    public static Vector2 left => leftVector;

//    //
//    // Résumé :
//    //     Shorthand for writing Vector2(1, 0).
//    public static Vector2 right => rightVector;

//    //
//    // Résumé :
//    //     Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).
//    public static Vector2 positiveInfinity => positiveInfinityVector;

//    //
//    // Résumé :
//    //     Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).
//    public static Vector2 negativeInfinity => negativeInfinityVector;

//    //
//    // Résumé :
//    //     Constructs a new vector with given x, y components.
//    //
//    // Paramètres :
//    //   x:
//    //
//    //   y:
//    public Vector2(float x, float y)
//    {
//        this.x = x;
//        this.y = y;
//    }

//    //
//    // Résumé :
//    //     Set x and y components of an existing Vector2.
//    //
//    // Paramètres :
//    //   newX:
//    //
//    //   newY:
//    public void Set(float newX, float newY)
//    {
//        x = newX;
//        y = newY;
//    }

//    //
//    // Résumé :
//    //     Linearly interpolates between vectors a and b by t.
//    //
//    // Paramètres :
//    //   a:
//    //
//    //   b:
//    //
//    //   t:
//    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
//    {
//        t = Mathf.Clamp01(t);
//        return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
//    }

//    //
//    // Résumé :
//    //     Linearly interpolates between vectors a and b by t.
//    //
//    // Paramètres :
//    //   a:
//    //
//    //   b:
//    //
//    //   t:
//    public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
//    {
//        return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
//    }

//    //
//    // Résumé :
//    //     Moves a point current towards target.
//    //
//    // Paramètres :
//    //   current:
//    //
//    //   target:
//    //
//    //   maxDistanceDelta:
//    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
//    {
//        float num = target.x - current.x;
//        float num2 = target.y - current.y;
//        float num3 = num * num + num2 * num2;
//        if (num3 == 0f || (maxDistanceDelta >= 0f && num3 <= maxDistanceDelta * maxDistanceDelta))
//        {
//            return target;
//        }

//        float num4 = (float)Math.Sqrt(num3);
//        return new Vector2(current.x + num / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
//    }

//    //
//    // Résumé :
//    //     Multiplies two vectors component-wise.
//    //
//    // Paramètres :
//    //   a:
//    //
//    //   b:
//    public static Vector2 Scale(Vector2 a, Vector2 b)
//    {
//        return new Vector2(a.x * b.x, a.y * b.y);
//    }

//    //
//    // Résumé :
//    //     Multiplies every component of this vector by the same component of scale.
//    //
//    // Paramètres :
//    //   scale:
//    public void Scale(Vector2 scale)
//    {
//        x *= scale.x;
//        y *= scale.y;
//    }

//    //
//    // Résumé :
//    //     Makes this vector have a magnitude of 1.
//    public void Normalize()
//    {
//        float num = magnitude;
//        if (num > 1E-05f)
//        {
//            this /= num;
//        }
//        else
//        {
//            this = zero;
//        }
//    }

//    //
//    // Résumé :
//    //     Returns a nicely formatted string for this vector.
//    //
//    // Paramètres :
//    //   format:
//    public override string ToString()
//    {
//        return $"{x}, {y}"; // UnityString.Format("({0:F1}, {1:F1})", x, y);
//    }

//    //
//    // Résumé :
//    //     Returns a nicely formatted string for this vector.
//    //
//    // Paramètres :
//    //   format:
//    //public string ToString(string format)
//    //{
//    //    return UnityString.Format("({0}, {1})", x.ToString(format, CultureInfo.InvariantCulture.NumberFormat), y.ToString(format, CultureInfo.InvariantCulture.NumberFormat));
//    //}

//    public override int GetHashCode()
//    {
//        return x.GetHashCode() ^ (y.GetHashCode() << 2);
//    }

//    //
//    // Résumé :
//    //     Returns true if the given vector is exactly equal to this vector.
//    //
//    // Paramètres :
//    //   other:
//    public override bool Equals(object other)
//    {
//        if (!(other is Vector2))
//        {
//            return false;
//        }

//        return Equals((Vector2)other);
//    }

//    public bool Equals(Vector2 other)
//    {
//        return x == other.x && y == other.y;
//    }

//    //
//    // Résumé :
//    //     Reflects a vector off the vector defined by a normal.
//    //
//    // Paramètres :
//    //   inDirection:
//    //
//    //   inNormal:
//    public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
//    {
//        float num = -2f * Dot(inNormal, inDirection);
//        return new Vector2(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
//    }

//    //
//    // Résumé :
//    //     Returns the 2D vector perpendicular to this 2D vector. The result is always rotated
//    //     90-degrees in a counter-clockwise direction for a 2D coordinate system where
//    //     the positive Y axis goes up.
//    //
//    // Paramètres :
//    //   inDirection:
//    //     The input direction.
//    //
//    // Retourne :
//    //     The perpendicular direction.
//    public static Vector2 Perpendicular(Vector2 inDirection)
//    {
//        return new Vector2(0f - inDirection.y, inDirection.x);
//    }

//    //
//    // Résumé :
//    //     Dot Product of two vectors.
//    //
//    // Paramètres :
//    //   lhs:
//    //
//    //   rhs:
//    public static float Dot(Vector2 lhs, Vector2 rhs)
//    {
//        return lhs.x * rhs.x + lhs.y * rhs.y;
//    }

//    //
//    // Résumé :
//    //     Returns the unsigned angle in degrees between from and to.
//    //
//    // Paramètres :
//    //   from:
//    //     The vector from which the angular difference is measured.
//    //
//    //   to:
//    //     The vector to which the angular difference is measured.
//    public static float Angle(Vector2 from, Vector2 to)
//    {
//        float num = (float)Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
//        if (num < 1E-15f)
//        {
//            return 0f;
//        }

//        float num2 = Mathf.Clamp(Dot(from, to) / num, -1f, 1f);
//        return (float)Math.Acos(num2) * 57.29578f;
//    }

//    //
//    // Résumé :
//    //     Returns the signed angle in degrees between from and to.
//    //
//    // Paramètres :
//    //   from:
//    //     The vector from which the angular difference is measured.
//    //
//    //   to:
//    //     The vector to which the angular difference is measured.
//    public static float SignedAngle(Vector2 from, Vector2 to)
//    {
//        float num = Angle(from, to);
//        float num2 = Math.Sign(from.x * to.y - from.y * to.x);
//        return num * num2;
//    }

//    //
//    // Résumé :
//    //     Returns the distance between a and b.
//    //
//    // Paramètres :
//    //   a:
//    //
//    //   b:
//    public static float Distance(Vector2 a, Vector2 b)
//    {
//        float num = a.x - b.x;
//        float num2 = a.y - b.y;
//        return (float)Math.Sqrt(num * num + num2 * num2);
//    }

//    //
//    // Résumé :
//    //     Returns a copy of vector with its magnitude clamped to maxLength.
//    //
//    // Paramètres :
//    //   vector:
//    //
//    //   maxLength:
//    public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
//    {
//        float num = vector.sqrMagnitude;
//        if (num > maxLength * maxLength)
//        {
//            float num2 = (float)Math.Sqrt(num);
//            float num3 = vector.x / num2;
//            float num4 = vector.y / num2;
//            return new Vector2(num3 * maxLength, num4 * maxLength);
//        }

//        return vector;
//    }

//    public static float SqrMagnitude(Vector2 a)
//    {
//        return a.x * a.x + a.y * a.y;
//    }

//    public float SqrMagnitude()
//    {
//        return x * x + y * y;
//    }

//    //
//    // Résumé :
//    //     Returns a vector that is made from the smallest components of two vectors.
//    //
//    // Paramètres :
//    //   lhs:
//    //
//    //   rhs:
//    public static Vector2 Min(Vector2 lhs, Vector2 rhs)
//    {
//        return new Vector2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
//    }

//    //
//    // Résumé :
//    //     Returns a vector that is made from the largest components of two vectors.
//    //
//    // Paramètres :
//    //   lhs:
//    //
//    //   rhs:
//    public static Vector2 Max(Vector2 lhs, Vector2 rhs)
//    {
//        return new Vector2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
//    }

//    public static Vector2 operator +(Vector2 a, Vector2 b)
//    {
//        return new Vector2(a.x + b.x, a.y + b.y);
//    }

//    public static Vector2 operator -(Vector2 a, Vector2 b)
//    {
//        return new Vector2(a.x - b.x, a.y - b.y);
//    }

//    public static Vector2 operator *(Vector2 a, Vector2 b)
//    {
//        return new Vector2(a.x * b.x, a.y * b.y);
//    }

//    public static Vector2 operator /(Vector2 a, Vector2 b)
//    {
//        return new Vector2(a.x / b.x, a.y / b.y);
//    }

//    public static Vector2 operator -(Vector2 a)
//    {
//        return new Vector2(0f - a.x, 0f - a.y);
//    }

//    public static Vector2 operator *(Vector2 a, float d)
//    {
//        return new Vector2(a.x * d, a.y * d);
//    }

//    public static Vector2 operator *(float d, Vector2 a)
//    {
//        return new Vector2(a.x * d, a.y * d);
//    }

//    public static Vector2 operator /(Vector2 a, float d)
//    {
//        return new Vector2(a.x / d, a.y / d);
//    }

//    public static bool operator ==(Vector2 lhs, Vector2 rhs)
//    {
//        float num = lhs.x - rhs.x;
//        float num2 = lhs.y - rhs.y;
//        return num * num + num2 * num2 < 9.99999944E-11f;
//    }

//    public static bool operator !=(Vector2 lhs, Vector2 rhs)
//    {
//        return !(lhs == rhs);
//    }
//}