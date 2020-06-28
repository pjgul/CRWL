using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRWL
{
    class Beastiary
    {
        //the health of the current enemy
        static public int MonsterHealth { get; set; }
        //the dmg of the current enemy can deal
        static public int MonsterAttack { get; set; }


        //HP: 10
        //DMG: 5
        //the function assign the values from the databse into the Beatiary properties
         static public void Putrid_Sludge()
        {
            using (var db = new DungeonContext())
            {
                
                var query = db.Monsters.Where(b => b.MonsterName == "Putrid Sludge").Select(b => b.MonsterHealth);
                foreach (int health in query)
                {
                    MonsterHealth = health;
                }
                var query1 = db.Monsters.Where(b => b.MonsterName == "Putrid Sludge").Select(b => b.MonsterAttack);
                foreach (int attack in query1)
                {
                    MonsterAttack = attack;
                }

            }
        }
    }
}
