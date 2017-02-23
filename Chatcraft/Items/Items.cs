using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    public static class Items
    {
        public static List<Item> _items;
        public static List<Item> ItemsList
        {
            get
            { 
                if (_items == null)
                { 
                List<Item> _list = new List<Item>();
                var itemList = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "//items//items.txt");
                foreach (var item in itemList)
                {
                    _list.Add(JsonConvert.DeserializeObject<Item>(item));
                }

                _items = _list;
                return _list;
                }

                else
                {
                    return _items;
                }
            }
        }

        public static Item GetItemById(int id)
        {
            if (Items.ItemsList.Any(i => i.id == id))
            {
                return ItemsList.FirstOrDefault(i => i.id == id);
            }
            else return Items.ItemsList[0];
        }

        public static string GetItemInfo(int id)
        {
            if (Items.ItemsList.Any(i => i.id == id)) { 
            Item item = GetItemById(id);
            var itemInfo = "Предмет: " + item.name;
            if (item.atk != 0) itemInfo += "\nУрон: " + item.atk.ToString("+#;-#;0");
            if (item.def != 0) itemInfo += "\nЗащита: " + item.def.ToString("+#;-#;0");
            if (item.mod_str != 0) itemInfo += "\nСила: " + item.mod_str.ToString("+#;-#;0");
            if (item.mod_dex != 0) itemInfo += "\nЛовкость: " + item.mod_dex.ToString("+#;-#;0");
            if (item.mod_int != 0) itemInfo += "\nИнтеллект: " + item.mod_int.ToString("+#;-#;0");
            if (item.mod_con != 0) itemInfo += "\nВыносливость: " + item.mod_con.ToString("+#;-#;0");
            if (item.mod_cha != 0) itemInfo += "\nХаризма: " + item.mod_cha.ToString("+#;-#;0");
            if (item.mod_luck != 0) itemInfo += "\nУдача: " + item.mod_luck.ToString("+#;-#;0");
            return itemInfo;
            } else { return "Предмет не найден в базе"; }
        }

        public static string GetShortItemInfo(int id)
        {
            if (Items.ItemsList.Any(i => i.id == id))
            {
                Item item = GetItemById(id);
                var itemInfo = "";
                if (item.atk != 0) itemInfo += "⚔" + item.atk.ToString("+#;-#;0");
                if (item.def != 0) itemInfo += "🛡" + item.def.ToString("+#;-#;0");
                if (item.mod_str != 0) itemInfo += "💪" + item.mod_str.ToString("+#;-#;0");
                if (item.mod_dex != 0) itemInfo += "🎯" + item.mod_dex.ToString("+#;-#;0");
                if (item.mod_int != 0) itemInfo += "📖" + item.mod_int.ToString("+#;-#;0");
                if (item.mod_con != 0) itemInfo += "🚜" + item.mod_con.ToString("+#;-#;0");
                if (item.mod_cha != 0) itemInfo += "🎭" + item.mod_cha.ToString("+#;-#;0");
                if (item.mod_luck != 0) itemInfo += "🎲" + item.mod_luck.ToString("+#;-#;0");
                return itemInfo;
            }
            else { return "Предмет не найден в базе"; }
        }

        public static string GetItemName(int id)
        {
            if (Items.ItemsList.Any(i => i.id == id))
            {
                return GetItemById(id).name;
            }
            else { return "Предмет не найден в базе"; }
        }
    }
}
