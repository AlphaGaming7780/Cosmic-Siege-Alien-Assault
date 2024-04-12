using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace K8055Velleman.Game
{
    internal static class Utility
    {
        public static IEnumerable<Type> GetAllSubclassOf(Type parent)
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var t in a.GetTypes())
                    if (t.IsSubclassOf(parent)) yield return t;
        }
    }
}
