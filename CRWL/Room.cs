using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRWL
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public ICollection<Monster> Monsters { get; set; }
    }
}
