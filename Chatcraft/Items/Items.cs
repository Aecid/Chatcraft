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
            if (Items.ItemsList.Any(i => i.Id == id))
            {
                return ItemsList.FirstOrDefault(i => i.Id == id);
            }
            else return Items.ItemsList[0];
        }

        public static string GetItemInfo(int id)
        {
            if (Items.ItemsList.Any(i => i.Id == id)) { 
            Item item = GetItemById(id);
            var itemInfo = "Предмет: " + item.Name;
            if (item.Atk != 0) itemInfo += "\nУрон: " + item.Atk.ToString("+#;-#;0");
            if (item.Def != 0) itemInfo += "\nЗащита: " + item.Def.ToString("+#;-#;0");
            if (item.ModStr != 0) itemInfo += "\nСила: " + item.ModStr.ToString("+#;-#;0");
            if (item.ModDex != 0) itemInfo += "\nЛовкость: " + item.ModDex.ToString("+#;-#;0");
            if (item.ModInt != 0) itemInfo += "\nИнтеллект: " + item.ModInt.ToString("+#;-#;0");
            if (item.ModCon != 0) itemInfo += "\nВыносливость: " + item.ModCon.ToString("+#;-#;0");
            if (item.ModCha != 0) itemInfo += "\nХаризма: " + item.ModCha.ToString("+#;-#;0");
            if (item.ModLuck != 0) itemInfo += "\nУдача: " + item.ModLuck.ToString("+#;-#;0");
            return itemInfo;
            } else { return "Предмет не найден в базе"; }
        }

        public static string GetShortItemInfo(int id)
        {
            if (Items.ItemsList.Any(i => i.Id == id))
            {
                Item item = GetItemById(id);
                var itemInfo = "";
                if (item.Atk != 0) itemInfo += "⚔" + item.Atk.ToString("+#;-#;0");
                if (item.Def != 0) itemInfo += "🛡" + item.Def.ToString("+#;-#;0");
                if (item.ModStr != 0) itemInfo += "💪" + item.ModStr.ToString("+#;-#;0");
                if (item.ModDex != 0) itemInfo += "🎯" + item.ModDex.ToString("+#;-#;0");
                if (item.ModInt != 0) itemInfo += "📖" + item.ModInt.ToString("+#;-#;0");
                if (item.ModCon != 0) itemInfo += "🚜" + item.ModCon.ToString("+#;-#;0");
                if (item.ModCha != 0) itemInfo += "🎭" + item.ModCha.ToString("+#;-#;0");
                if (item.ModLuck != 0) itemInfo += "🎲" + item.ModLuck.ToString("+#;-#;0");
                return itemInfo;
            }
            else { return "Предмет не найден в базе"; }
        }

        public static string GetItemName(int id)
        {
            if (Items.ItemsList.Any(i => i.Id == id))
            {
                return GetItemById(id).Name;
            }
            else { return "Предмет не найден в базе"; }
        }
    }
}
