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
        public int Money { get; set; } = 0;
        public Dictionary<string, StratagemData> StratagemsData { get; set; } = [];


        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is not PlayerData)
            {
                return false;
            }

            return Equals((PlayerData)other);
        }

        public bool Equals(PlayerData other)
        {
            return Name == other.Name;
        }

        public static bool operator ==(PlayerData lhs, PlayerData rhs)
        {
            return lhs?.Name == rhs?.Name;
        }

        public static bool operator !=(PlayerData lhs, PlayerData rhs)
        {
            return lhs?.Name != rhs?.Name;
        }
    }
}
