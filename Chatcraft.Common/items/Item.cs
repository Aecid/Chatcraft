using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    /// <summary>
    /// Предмет
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Id предмета
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя предмета
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// Необходимый уровень вещи
        /// </summary>
        public int LvlReq { get; set; }
        /// <summary>
        /// Сила атаки
        /// </summary>
        public int Atk { get; set; }
        /// <summary>
        /// Защита
        /// </summary>
        public int Def { get; set; }
        /// <summary>
        /// ??
        /// </summary>
        public int ModStr { get; set; }
        /// <summary>
        /// ??
        /// </summary>
        public int ModInt { get; set; }
        /// <summary>
        /// ??
        /// </summary>
        public int ModDex { get; set; }
        /// <summary>
        /// ??
        /// </summary>
        public int ModCon { get; set; }
        /// <summary>
        /// ??
        /// </summary>
        public int ModCha { get; set; }
        /// <summary>
        /// Удача
        /// </summary>
        public int ModLuck { get; set; }
        /// <summary>
        /// Находится ли вещь в снаряжении?
        /// </summary>
        public bool IsEquipment { get; set; }
        /// <summary>
        /// ?? (Слот)?
        /// </summary>
        public string Slot { get; set;}
        /// <summary>
        /// Может ли быть куплена 
        /// </summary>
        public bool CanBeBought { get; set; }
        /// <summary>
        /// Может ли выпасть из моба?
        /// </summary>
        public bool CanBeLooted { get; set; }
    }
}
