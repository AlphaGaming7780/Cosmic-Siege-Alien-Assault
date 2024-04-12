using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman
{
    internal static class SizeExtention
    {
        public static Size Multiply(this Size size, int m)
        {
            size.Width *= m;
            size.Height *= m;
            return size;
        }

        public static Size Divide(this Size size, int d) 
        {
            size.Width /= d;
            size.Height /= d;
            return size;
        }
    }
}
