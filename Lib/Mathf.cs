using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman
{
    internal class Mathf
    {
        public static float Clamp(float val, float min, float max)
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static float Clamp01(float val)
        {
            return Clamp(val, 0.0f, 1.0f);
        }

    }
}
