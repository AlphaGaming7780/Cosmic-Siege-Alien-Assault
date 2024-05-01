using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Saves
{
    public class PlayerData
    {
        public string Name { get; set; } = "NO NAMES";
        public int HigestScore { get; set; } = 0;
        public Dictionary<string, StratagemData> StratagemsData { get; set; } = [];
    }
}
