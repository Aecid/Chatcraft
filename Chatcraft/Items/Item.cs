using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int lvlReq { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public int mod_str { get; set; }
        public int mod_int { get; set; }
        public int mod_dex { get; set; }
        public int mod_con { get; set; }
        public int mod_cha { get; set; }
        public int mod_luck { get; set; }
        public bool isEquipment { get; set; }
        public string slot { get; set;}
        public bool canBeBought { get; set; }
        public bool canBeLooted { get; set; }
    }
}
