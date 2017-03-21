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
    /// <summary>
    /// Статический класс, в котором хранятся вещи (полученные из тесктового файла)
    /// Есть словарь вещей и идентичный ему список вещей.
    /// </summary>
    public static class Items
    {
        static Items()
        {
            foreach (var item in ItemsList)
                ItemsDict.Add(item.Id, item);
        }
        /// <summary>
        /// Словарь с вещами. Ключ- id вещи
        /// </summary>
        public static SortedDictionary<int, Item> ItemsDict = new SortedDictionary<int, Item>();

        public static List<Item> _items;
        /// <summary>
        /// Список предметов
        /// </summary>
        public static List<Item> ItemsList
        {
            get
            { 
                if (_items == null)
                { 
                List<Item> _list = new List<Item>();
                foreach (var item in File.ReadAllLines(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "//items//items.txt"))
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
        /// <summary>
        /// Получить предмет по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Item GetItemById(int id)
        {
            Item item = null;
            if(ItemsDict.TryGetValue(id, out item))
                return item;
            else
            {
                return Items.ItemsList[0];//не нашли- берем самую первую вещь
            }            
        }
        /// <summary>
        /// Получить информацию о предмете
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetItemInfo(int id)
        {
            Item item = GetItemById(id);
            if (item.Id == id)
            {
                var itemInfo = new StringBuilder();
                itemInfo.Append($"Предмет: {item.Name}");
                if (item.Atk != 0) itemInfo.Append($"\nУрон: {item.Atk.ToString("+#;-#;0")}");
                if (item.Def != 0) itemInfo.Append($"\nЗащита: {item.Def.ToString("+#;-#;0")}");
                if (item.ModStr != 0) itemInfo.Append($"\nСила: {item.ModStr.ToString("+#;-#;0")}");
                if (item.ModDex != 0) itemInfo.Append($"\nЛовкость: {item.ModDex.ToString("+#;-#;0")}");
                if (item.ModInt != 0) itemInfo.Append($"\nИнтеллект: {item.ModInt.ToString("+#;-#;0")}");
                if (item.ModCon != 0) itemInfo.Append($"\nВыносливость: {item.ModCon.ToString("+#;-#;0")}");
                if (item.ModCha != 0) itemInfo.Append($"\nХаризма: {item.ModCha.ToString("+#;-#;0")}");
                if (item.ModLuck != 0) itemInfo.Append($"\nУдача: {item.ModLuck.ToString("+#;-#;0")}");
                return itemInfo.ToString();
            }
            else { return "Предмет не найден в базе"; }
        }

        public static string GetShortItemInfo(int id)
        {
            Item item = GetItemById(id);
            if (item.Id == id)
            {
                var itemInfo = new StringBuilder();
                if (item.Atk != 0) itemInfo.Append($"⚔{item.Atk.ToString("+#;-#;0")}");
                if (item.Def != 0) itemInfo.Append($"🛡{item.Def.ToString("+#;-#;0")}");
                if (item.ModStr != 0) itemInfo.Append($"💪{item.ModStr.ToString("+#;-#;0")}");
                if (item.ModDex != 0) itemInfo.Append($"🎯{item.ModDex.ToString("+#;-#;0")}");
                if (item.ModInt != 0) itemInfo.Append($"📖{item.ModInt.ToString("+#;-#;0")}");
                if (item.ModCon != 0) itemInfo.Append($"🚜{item.ModCon.ToString("+#;-#;0")}");
                if (item.ModCha != 0) itemInfo.Append($"🎭{item.ModCha.ToString("+#;-#;0")}");
                if (item.ModLuck != 0) itemInfo.Append($"🎲{item.ModLuck.ToString("+#;-#;0")}");
                return itemInfo.ToString();
            }
            else { return "Предмет не найден в базе"; }
        }

        /// <summary>
        /// Получить имя вещи по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetItemName(int id)
        {
            Item item = null;
            ItemsDict.TryGetValue(id, out item);
            if (item != null)
                return item.Name;
            else
                return "Предмет не найден в базе";            
        }
    }
}
