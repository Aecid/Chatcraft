using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    /// <summary>
    /// Mob class
    /// </summary>
    public class Mob
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Level
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// Picture
        /// </summary>
        public string Pic { get; set; }
        /// <summary>
        /// Atk
        /// </summary>
        public int Atk { get; set; }
        /// <summary>
        /// Defense
        /// </summary>
        public int Def { get; set; }
        /// <summary>
        /// Health points
        /// </summary>
        public long HP { get; set; }
        //<item id, drop percentage>
        public Dictionary<int, int> LootTable { get; set; }
        /// <summary>
        /// Mob constructor
        /// </summary>
        /// <param name="id">unic id</param>
        /// <param name="level">level</param>
        /// <param name="name">name</param>
        /// <param name="desc">description</param>
        /// <param name="atk"></param>
        /// <param name="def">defense</param>
        /// <param name="hp">helth points</param>
        /// <param name="lootTable">loot table</param>
        /// <param name="pic">picture</param>
        public Mob(int id, int level, string name, string desc, int atk, int def, long hp, Dictionary<int, int> lootTable, string pic = "")
        {
            Id = id;
            Level = level;
            Name = name;
            Desc = desc;
            Pic = pic;
            Atk = atk;
            Def = def;
            HP = hp;
            LootTable = lootTable;
        }
    }
}
