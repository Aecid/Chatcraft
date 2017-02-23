using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    public class Mob
    {
        public int id { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public string pic { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public long hp { get; set; }
        //<item id, drop percentage>
        public Dictionary<int, int> lootTable { get; set; }

        public Mob(int _id, int _level, string _name, string _desc, int _atk, int _def, long _hp, Dictionary<int, int> _lootTable, string _pic = "")
        {
            id = _id;
            level = _level;
            name = _name;
            desc = _desc;
            pic = _pic;
            atk = _atk;
            def = _def;
            hp = _hp;
            lootTable = _lootTable;
        }
    }
}
