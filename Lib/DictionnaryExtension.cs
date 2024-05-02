using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Lib;

internal static class DictionnaryExtension
{
    public static void AddOrSet<K, V>(this Dictionary<K, V> dict, K key, V value)
    {
        if(dict.ContainsKey(key)) dict[key] = value;
        else dict.Add(key, value);
    }
}
