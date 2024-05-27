using System;
using System.Collections.Generic;

namespace K8055Velleman.Game
{
    internal static class Utility
    {
        /// <summary>
        /// Get all the sub class of a class.
        /// </summary>
        /// <param name="parent">The type of the parent class.</param>
        /// <returns>An IEnumerable of Type.</returns>
        public static IEnumerable<Type> GetAllSubclassOf(Type parent)
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var t in a.GetTypes())
                    if (t.IsSubclassOf(parent)) yield return t;
        }
    }
}
