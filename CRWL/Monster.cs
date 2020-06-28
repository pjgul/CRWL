using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CRWL
{
    public class Monster
    {
        public int MonsterId { get; set; }
        public string MonsterName { get; set; }
        public int MonsterHealth { get; set; }
        public int MonsterAttack { get; set; }
        public Room Room { get; set; }
    }
}
