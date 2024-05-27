

namespace K8055Velleman
{
    internal class Mathf
    {
        /// <summary>
        /// Clamp a float value between a min and a max.
        /// </summary>
        /// <param name="val">The value to clamp.</param>
        /// <param name="min">The min value of the clamp.</param>
        /// <param name="max">The max value of the clamp.</param>
        /// <returns></returns>
        public static float Clamp(float val, float min, float max)
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        /// <summary>
        /// Clamp a float between 0 and 1.
        /// </summary>
        /// <param name="val">The value to clamp.</param>
        /// <returns></returns>
        public static float Clamp01(float val)
        {
            return Clamp(val, 0.0f, 1.0f);
        }

    }
}
