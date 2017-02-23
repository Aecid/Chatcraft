using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace Chatcraft
{
    public class Session
    {
        public long id { get; set; }
        public string title { get; set; }
        public string username { get; set; }
        public string pageId { get; set; }
        public string name { get; set; }
        public bool inQuest { get; set; }
        public long exp { get; set; }
        public int level { get; set; }
        public long gold { get; set; }
        public long hp { get; set; }
        public long maxHP { get; set; }
        public long mp { get; set; }
        public long maxMP { get; set; }
        public int strength { get; set; }
        public int dexterity { get; set; }
        public int intellect { get; set; }
        public int constitution { get; set; }
        public int charisma { get; set; }
        public int luck { get; set; }
        public List<int> items { get; set; }
        public int attributePoints { get; set; }
        public List<int> achievements { get; set; }
        public SortedDictionary<string, int> statistic { get; set; }
        public Dictionary<string, object> equipment;
        public bool gender { get; set; }
        public int guild;

        public Session()
        {
            inQuest = false;
            exp = 0;
            level = 1;
            gold = 0;
            hp = 10;

            strength = 1;
            dexterity = 1;
            intellect = 1;
            constitution = 1;
            charisma = 0;
            luck = 0;

            maxHP = constitution * 10;
            maxMP = intellect * 10;

            items = new List<int>();
            statistic = new SortedDictionary<string, int>();
            equipment = new Dictionary<string, object>();
            achievements = new List<int>();
        }

        public int GetLevel()
        {
            GetExp();
            var char_level = level;
            return char_level;
        }
        public long GetHP()
        {
            var char_maxHP = GetMaxHP();
            hp = hp > char_maxHP ? char_maxHP : hp;
            return hp;
        }

        public long GetMP()
        {
            var char_maxMP = GetMaxMP();
            mp = mp > char_maxMP ? char_maxMP : mp;
            return mp;
        }

        public long GetMaxHP()
        {
            var char_maxHP = GetConst() * 10;
            hp = hp > char_maxHP ? char_maxHP : hp;
            return char_maxHP;
        }

        public long GetMaxMP()
        {
            var char_maxMP = GetInt() * 10;
            mp = mp > char_maxMP ? char_maxMP : mp;
            return char_maxMP;
        }

        public int GetStr()
        {
            var str = strength;
            foreach (var equip in equipment)
            {
                str += Items.GetItemById(int.Parse(equip.Value.ToString())).mod_str;
            }
            return str;
        }

        public int GetInt()
        {
            var inta = intellect;
            foreach (var equip in equipment)
            {
                inta += Items.GetItemById(int.Parse(equip.Value.ToString())).mod_int;
            }
            return inta;
        }

        public int GetConst()
        {
            var cons = constitution;
            foreach (var equip in equipment)
            {
                cons += Items.GetItemById(int.Parse(equip.Value.ToString())).mod_con;
            }
            return cons;
        }

        public int GetDex()
        {
            var dex = dexterity;
            foreach (var equip in equipment)
            {
                dex += Items.GetItemById(int.Parse(equip.Value.ToString())).mod_dex;
            }
            return dex;
        }

        public int GetCha()
        {
            var cha = charisma;
            foreach (var equip in equipment)
            {
                cha += Items.GetItemById(int.Parse(equip.Value.ToString())).mod_cha;
            }
            return cha;
        }

        public int GetLuck()
        {
            var _luck = luck;
            foreach (var equip in equipment)
            {
                _luck += Items.GetItemById(int.Parse(equip.Value.ToString())).mod_luck;
            }
            return _luck;
        }

        public long GetExp()
        {
            return exp;
        }

        public long GetExpTNL()
        {
            if (level == 1) return 42;
            double num = 0;
            for (int x = 1; x < level; x++)
            {
                double aa = (x + 300 * Math.Pow(2, (x / 17.0)));
                num += Math.Floor(aa);
            }
            int _exp = (int)Math.Floor(num / 4);

            return _exp;
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetStatus()
        {
            Persist();
            //Dictionary<string, string> left = new Dictionary<string, string>()
            //{
            //    { "👤Имя", name },
            //    { "♥️Здоровье", GetHP()+"/"+GetMaxHP() },
            //    { "🔮Мана", GetMP()+"/"+GetMaxMP() },
            //    { "⭐️Уровень", GetLevel().ToString() },
            //    { "🔥Опыт", GetExp()+"/"+GetExpTNL() },
            //    { "💰Золото", gold.ToString() },
            //    { "aaa", "%divider%" },
            //    { "Урон", GetAttackString() },
            //    { "Защита", GetDefenseString() }
            //};

            //Dictionary<string, string> right = new Dictionary<string, string>()
            //{
            //    { "💪Сила", GetStr().ToString() },
            //    { "🎯Ловкость", GetDex().ToString()},
            //    { "📖Интеллект", GetInt().ToString()},
            //    { "🚜Выносливость", GetConst().ToString()},
            //    { "🎭Харизма", GetCha().ToString() },
            //    { "🎲Удача", GetLuck().ToString() }
            //};

            Dictionary<string, string> left = new Dictionary<string, string>()
            {
                { "Имя", GetTitle() + " " + name },
                { "Здоровье", GetHP()+"/"+GetMaxHP() },
                { "Мана", GetMP()+"/"+GetMaxMP() },
                { "Уровень", GetLevel().ToString() },
                { "Опыт", GetExp()+"/"+GetExpTNL() },
                { "Золото", gold.ToString() },
                { "aaa", "%divider%" },
                { "Урон", GetAttackString().Replace("⚔", "") },
                { "Защита", GetDefenseString().Replace("🛡", "") },
                { "bbb", "%divider%"},
                { "Сила", GetStr().ToString() },
                { "Ловкость", GetDex().ToString()},
                { "Интеллект", GetInt().ToString()},
                { "Выносливость", GetConst().ToString()},
                { "Харизма", GetCha().ToString() },
                { "Удача", GetLuck().ToString() }
            };

            Dictionary<string, string> right = new Dictionary<string, string>()
            {
                { "Сила", GetStr().ToString() },
                { "Ловкость", GetDex().ToString()},
                { "Интеллект", GetInt().ToString()},
                { "Выносливость", GetConst().ToString()},
                { "Харизма", GetCha().ToString() },
                { "Удача", GetLuck().ToString() }
            };

            return "<pre>" + Helper.FormattedTable(left) + "</pre>";
            ///return string.Format("⭐️({0}) {1} {2}|♥️{3}/{4}|🔮{5}/{6}🔥{7}|💰{8}|", GetLevel(), GetTitle(), name, GetHP(), GetMaxHP(), GetMP(), GetMaxMP(), exp, gold);
        }

        public string GetAlienStatus()
        {
            string equip = "";
            foreach (var item in equipment)
            {
                equip += Items.GetItemName((int)item.Value) + "|";
            }
            return string.Format("⭐️({0}) {1} {2}|♥️{3}|🔮{4}|{5}", GetLevel(), GetTitle(), name, GetMaxHP(), GetMaxMP(), equip);
        }

        public string GetState()
        {
            return "Баклуши бьёт";
        }

        public string GetFullStatus()
        {
            Persist();
            string itemList = "";
            if (items != null)
            {
                var stackedItems = items.GroupBy(i => i).ToDictionary(s => s.Key, s => s.Count());
                foreach (var itemId in stackedItems)
                {
                    var item = Items.GetItemById(itemId.Key);
                    var stats = "";
                    stats += item.atk > 0 ? " ⚔" + item.atk : "";
                    stats += item.def > 0 ? " 🛡" + item.def : "";
                    string itemCount = itemId.Value > 1 ? " (" + itemId.Value + ")" : ""; 
                    itemList += "\n" + item.name + itemCount + stats + " /on_" + item.id;
                }
            }
            string equipList = "";
            if (equipment != null)
            {

                if (equipment.Any(e => e.Key == "slotRightHand" || e.Key == "slotBothHands"))
                {
                    var item = Items.GetItemById(int.Parse(equipment.FirstOrDefault(e => e.Key == "slotRightHand" || e.Key == "slotBothHands").Value.ToString()));
                    equipList += item.slot.Equals("slotRightHand") ? "\n<i>Правая рука:</i>\n" + "<b>" + item.name + "</b>" : "\n<i>Обе руки:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }
                if (equipment.Keys.Contains("slotLeftHand"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotLeftHand"].ToString()));
                    equipList += "\n<i>Левая рука:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }
                if (equipment.Keys.Contains("slotFace"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotFace"].ToString()));
                    equipList += "\n<i>Лицо:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }
                if (equipment.Keys.Contains("slotHead"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotHead"].ToString()));
                    equipList += "\n<i>Голова:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }
                if (equipment.Keys.Contains("slotChest"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotChest"].ToString()));
                    equipList += "\n<i>Тело:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }
                if (equipment.Keys.Contains("slotBack"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotBack"].ToString()));
                    equipList += "\n<i>Плащ:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }
                if (equipment.Keys.Contains("slotHands"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotHands"].ToString()));
                    equipList += "\n<i>Плечи:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }
                if (equipment.Keys.Contains("slotArms"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotArms"].ToString()));
                    equipList += "\n<i>Перчатки:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }
                if (equipment.Keys.Contains("slotLegs"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotLegs"].ToString()));
                    equipList += "\n<i>Ноги:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }
                if (equipment.Keys.Contains("slotFeet"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotFeet"].ToString()));
                    equipList += "\n<i>Ботинки:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                }

            }
            string levelUp = "";
            if (attributePoints > 0)
            {
                levelUp += "<b>----------------------------</b>\n<b>Свободные очки характеристик: [" + attributePoints + "]\nДля повышения уровня используйте команду</b> /levelUp\n";
            }

            return string.Format(@"{0}
👤<b>Имя: {1} {2}</b>
♥️Здоровье: {3}/{4}
🔮Мана: {5}/{6}
⭐️Уровень: {7}
🔥Опыт: {8}/{18}
💰Золото: {9}
 <b>----------------------------</b>
👊<b>Характеристики:</b>
{20}
{21}
<b>----------------------------</b>
💪Сила: {10}
🎯Ловкость: {11}
📖Интеллект: {12}
🚜Выносливость: {13}
🎭Харизма: {14}
🎲Удача: {15}
{19}<b>----------------------------</b>
🎩<b>Экипировка:</b>{17}", "", GetTitle(), name, GetHP(), GetMaxHP(), GetMP(), GetMaxMP(), GetLevel(), GetExp(), gold, GetStr(), GetDex(), GetInt(), GetConst(), GetCha(), GetLuck(), itemList, equipList, GetExpTNL(), levelUp, GetAttackString(), GetDefenseString(), items.Count());
        }

        internal async void SetName(string desiredName)
        {
            if (name.Length > 2 && name.Length <= 32 && Regex.IsMatch(desiredName, @"^[a-zA-Zа-яА-Я0-9]+$")) {
            name = desiredName;
            Persist();
                string title = "Приветствую, ";
                    title += gender ? "сэр " : "леди ";
                title += name;
                await SendMessage(title);
            } else
            {
                await SendMessage("Имя должно быть от 3 до 32 символов в длину и не содержать специальных символов.");
            }
        }

        internal async void ShowOptions()
        {
            string optionsMessage = @"Доступные опции:
Смена имени /name {имя} (Установленное имя персонажа: "+name+@") 
Смена пола /gender";
            await SendMessage(optionsMessage);
        }

        public void Persist()
        {
            Helper.WriteToJsonFile(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\chars\\" + id + ".json", this);
        }

        public async Task SendPhoto(string photo, string message)
        {
            await BotClient.Instance.SendChatActionAsync(id, ChatAction.UploadPhoto);

            string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\img\\" + photo;

            var fileName = file.Split('\\').Last();

            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fts = new FileToSend(fileName, fileStream);

                await BotClient.Instance.SendPhotoAsync(id, fts, message);
            }
        }

        public async Task SendKeyboard(ReplyKeyboardMarkup keyboard, string message)
        {
            await BotClient.Instance.SendTextMessageAsync(id, message, replyMarkup: keyboard, parseMode: ParseMode.Html);
        }

        public async Task SendInlineKeyboard(InlineKeyboardMarkup keyboard, string message)
        {
            await BotClient.Instance.SendTextMessageAsync(id, message, replyMarkup: keyboard, parseMode: ParseMode.Html);
        }

        public async Task SendMessage(string message, ReplyKeyboardMarkup keyboard = null, string photo = null)
        {
            if (photo != null && keyboard != null)
            {
                await BotClient.Instance.SendChatActionAsync(id, ChatAction.UploadPhoto);

                string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\img\\" + photo;

                var fileName = file.Split('\\').Last();

                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fts = new FileToSend(fileName, fileStream);

                    await BotClient.Instance.SendPhotoAsync(id, fts, message, replyMarkup: keyboard);
                }
            }

            if (photo == null && keyboard != null)
            {
                SendKeyboard(keyboard, message);
            }

            if (photo != null && keyboard == null)
            {
                SendPhoto(photo, message);
            }

            if (photo == null && keyboard == null)
            {
                if (message != null)
                {
                    await BotClient.Instance.SendTextMessageAsync(id, message, parseMode: ParseMode.Html);
                }
            }
        }

        public async void SendInlineMessage(string message, InlineKeyboardMarkup keyboard = null, string photo = null)
        {
            if (photo != null && keyboard != null)
            {
                await BotClient.Instance.SendChatActionAsync(id, ChatAction.UploadPhoto);

                string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\img\\" + photo;

                var fileName = file.Split('\\').Last();

                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fts = new FileToSend(fileName, fileStream);

                    await BotClient.Instance.SendPhotoAsync(id, fts, message, replyMarkup: keyboard);
                }
            }

            if (photo == null && keyboard != null)
            {
                SendInlineKeyboard(keyboard, message);
            }

            if (photo != null && keyboard == null)
            {
                SendPhoto(photo, message);
            }

            if (photo == null && keyboard == null)
            {
                await BotClient.Instance.SendTextMessageAsync(id, message);
            }
        }

        public void GotoPage(string _pageId)
        {
            pageId = _pageId;
        }

        public void StartQuest(string quest)
        {
            if (!inQuest)
            {
                switch (quest)
                {
                    case "Лес":
                        ForestQuest();
                        break;
                    case "Пещера":
                        CaveQuest();
                        break;
                    default:
                        ForestQuest();
                        break;
                }
            }
            else
            {
                SendMessage("Вы уже на задании.");
            }
        }

        public void ForestQuest()
        {
            QuestsPage.ForestQuest(this);
        }
        public void CaveQuest()
        {
            QuestsPage.CaveQuest(this);
        }

        public void AddGold(long amount)
        {
            gold = gold + amount;
            gold = gold < 0 ? 0 : gold;
        }

        public void AddExp(long amount)
        {
            exp = exp + amount;
            exp = exp < 0 ? 0 : exp;
            if (exp >= GetExpTNL())
            {
                var _exp = exp - GetExpTNL();

                //levelup
                level++;
                //constitution++;
                attributePoints++;
                LevelUp();
                exp = _exp;
            }
        }

        public void Heal(long amount)
        {
            hp = hp + amount;
            hp = hp > GetMaxHP() ? GetMaxHP() : hp;
        }

        public void Mana(long amount)
        {
            mp = mp + amount;
            mp = mp > GetMaxMP() ? GetMaxMP() : mp;
        }

        public void DealDamage(long amount)
        {
            hp = hp - amount;
            hp = hp < 0 ? 0 : hp;
            if (hp <= 0)
            {
                Die();
            }
        }

        internal void AddItem(List<int> addItems)
        {
            foreach (var item in addItems)
            {
                if (items == null)
                {
                    items = new List<int>();
                }

                items.Add(item);
            }
            Persist();
        }

        public void EquipItem(int id)
        {
            if (equipment == null) equipment = new Dictionary<string, object>();
            if (items != null && items.Contains(id))
            {
                var item = Items.GetItemById(id);
                items.Remove(id);

                switch (item.slot)
                {
                    case "slotRightHand":
                        if (equipment.ContainsKey(item.slot))
                        {
                            UnequipItem(item.slot);
                        }

                        if (equipment.ContainsKey("slotBothHands"))
                        {
                            UnequipItem("slotBothHands");
                        }
                        equipment[item.slot] = id;
                        break;
                    case "slotLeftHand":
                        if (equipment.ContainsKey(item.slot))
                        {
                            UnequipItem(item.slot);
                        }

                        if (equipment.ContainsKey("slotBothHands"))
                        {
                            UnequipItem("slotBothHands");
                        }
                        equipment[item.slot] = id;
                        break;
                    case "slotBothHands":
                        if (equipment.ContainsKey(item.slot))
                        {
                            UnequipItem(item.slot);
                        }

                        if (equipment.ContainsKey("slotRightHand"))
                        {
                            UnequipItem("slotRightHand");
                        }

                        if (equipment.ContainsKey("slotLeftHand"))
                        {
                            UnequipItem("slotLeftHand");
                        }
                        equipment[item.slot] = id;
                        break;
                    case "title":
                        break;
                    case "rings":
                        break;
                    default:
                        if (equipment.Keys.Contains(item.slot)) UnequipItem(int.Parse(equipment[item.slot].ToString()));
                        equipment[item.slot] = id;
                        break;
                }

                SendMessage("Экипировано: " + item.name);
            }

            else
            {
                SendMessage("Предмет не найден");
            }
            Persist();
        }

        public void UnequipItem(int id)
        {
            if (equipment == null) equipment = new Dictionary<string, object>();
            if (items == null) items = new List<int>();
            if (Items.ItemsList.Any(i => i.id == id))
            {
                var item = Items.GetItemById(id);

                if (equipment.Any(e => int.Parse(e.Value.ToString()) == id))
                {
                    items.Add(id);
                    equipment.Remove(item.slot);
                    SendMessage("Снято: " + item.name);
                }
                else
                {
                    SendMessage("Предмет не найден");
                }
                Persist();
            }
        }

        public void UnequipItem(string slot)
        {
            if (equipment == null) equipment = new Dictionary<string, object>();
            if (items == null) items = new List<int>();

            if (equipment.Keys.Contains(slot))
            {
                var itemId = int.Parse(equipment[slot].ToString());
                items.Add(itemId);
                equipment.Remove(slot);
                SendMessage("Снято: " + Items.GetItemById(itemId).name);
            }
            else
            {
                SendMessage("Предмет не найден");
            }
            Persist();
        }

        public void BuyItem(int id)
        {
            if (items == null) items = new List<int>();
            if (Items.ItemsList.Any(i => i.id == id))
            {
                var item = Items.GetItemById(id);

                if (gold < item.price)
                {
                    SendMessage("Недостаточно золота.");
                }

                else
                {
                    gold -= item.price;
                    items.Add(id);
                    SendMessage("Вы купили " + item.name);
                }
                Persist();
            }
        }

        public void SellItem(int id)
        {
            if (items == null) items = new List<int>();
            if (Items.ItemsList.Any(i => i.id == id))
            {

                var item = Items.GetItemById(id);

                if (items.Contains(id))
                {
                    gold += item.price / 3;
                    items.Remove(id);
                    SendMessage("Вы продали " + item.name);
                }
                else
                {
                    SendMessage("У вас нет предмета " + item.name);
                }
                Persist();
            }
        }

        public void LevelUp()
        {
            SendMessage("🏆Поздравляем, вы получили новый уровень!🏆\nИспользуй /levelUp для повышения выбранной характеристики.");
        }

        public void AddAttribute(string attribute)
        {
            if (attributePoints > 0)
            {
                switch (attribute)
                {
                    case "💪Сила":
                        strength++;
                        attributePoints--;
                        string msg = "Добавлено +1 очко к " + attribute;
                        msg += attributePoints > 0 ? "\nУ вас [" + attributePoints + "] свободных очков характеристик" : "";
                        SendMessage(msg);
                        break;
                    case "📖Интеллект":
                        intellect++;
                        attributePoints--;
                        msg = "Добавлено +1 очко к " + attribute;
                        msg += attributePoints > 0 ? "\nУ вас [" + attributePoints + "] свободных очков характеристик" : "";
                        SendMessage(msg);
                        break;
                    case "🎯Ловкость":
                        dexterity++;
                        attributePoints--;
                        msg = "Добавлено +1 очко к " + attribute;
                        msg += attributePoints > 0 ? "\nУ вас [" + attributePoints + "] свободных очков характеристик" : "";
                        SendMessage(msg);
                        break;
                    case "🚜Выносливость":
                        constitution++;
                        attributePoints--;
                        msg = "Добавлено +1 очко к " + attribute;
                        msg += attributePoints > 0 ? "\nУ вас [" + attributePoints + "] свободных очков характеристик" : "";
                        SendMessage(msg);
                        break;
                    default:
                        SendMessage("Произошло некоторое дерьмо.");
                        break;
                }
                Persist();
            }

            else
            {
                SendMessage("Не хватает свободных очков опыта");
            }
        }

        public int GetNativeAttack()
        {
            int atk =  GetStr() / 2;
            if (atk == 0) atk = 1;
            return atk;
        }

        public int GetItemsAttack()
        {
            int attack = 0;
            foreach (var equippedItem in equipment)
            {
                if (!equippedItem.Key.Equals("rings"))
                {
                    var item = Items.GetItemById(int.Parse(equippedItem.Value.ToString()));
                    attack += item.atk;
                }
            }
            return attack;
        }

        public int GetAttack()
        {
            return GetNativeAttack() + GetItemsAttack();
        }

        public string GetAttackString()
        {
            return "⚔" + GetAttack() + " (" + GetNativeAttack() + "+" + GetItemsAttack() + ")";
        }

        public int GetNativeDefense()
        {
            return GetDex() / 2;
        }

        public int GetItemsDefense()
        {
            int defense = 0;
            foreach (var equippedItem in equipment)
            {
                if (!equippedItem.Key.Equals("rings"))
                {
                    var item = Items.GetItemById(int.Parse(equippedItem.Value.ToString()));
                    defense += item.def;
                }
            }
            return defense;
        }

        public int GetDefense()
        {
            return GetNativeDefense() + GetItemsDefense();
        }

        public string GetDefenseString()
        {
            return "🛡" + GetDefense() + " (" + GetNativeDefense() + "+" + GetItemsDefense() + ")";
        }

        public void AddStatsCounter(string stat)
        {
            if (statistic.ContainsKey(stat))
            {
                statistic[stat]++;
                if (Achievements.AchList.Any(a => a.stat.Equals(stat)))
                {
                    if (achievements == null) achievements = new List<int>();
                    var achi = Achievements.GetAchByStatName(stat);
                    if (!achievements.Contains(achi.id))
                    {
                        if (statistic[stat] >= achi.count)
                        {
                            achievements.Add(achi.id);
                            SendMessage("Новое достижение - 🏆<b>" + achi.name + "</b>🏆!\n" + achi.description);
                        }
                    }
                }
            }
            else
            {
                statistic[stat] = 1;
            }
        }

        public InlineKeyboardMarkup GetStatusKeyboard()
        {
            List<string> options = new List<string>();
            //if (items.Count > 0) options.Add("Рюкзак");
            if (achievements.Count > 0) options.Add("Достижения");
            if (statistic.Count > 0) options.Add("Статистика");
            options.Add("Опции");
            return Helper.GetVerticalInlineKeyboardByList(options);
        }

        public async void ShowStats()
        {
            string result = "";
            
            foreach (var stat in statistic)
            {
                result += "<i>" + stat.Key + ": " + stat.Value + "</i>\n";
            }

            await SendMessage(result);
        }

        public async void ShowAchievements()
        {
            string result = "";
            foreach (var achi in achievements)
            {
                var ac = Achievements.GetAchById(achi);
                result += "🏆" + ac.name + ": " + ac.description + " /setTitle_" + ac.id + "\n";
            }
            result += "Убрать титул - /setTitle_0";
            await SendMessage(result);
        }

        public async void ShowBackpack()
        {
            string itemList = "";
            if (items != null)
            {
                var stackedItems = items.GroupBy(i => i).ToDictionary(s => s.Key, s => s.Count());
                foreach (var itemId in stackedItems)
                {
                    var item = Items.GetItemById(itemId.Key);
                    var stats = "";
                    stats += item.atk > 0 ? " ⚔" + item.atk : "";
                    stats += item.def > 0 ? " 🛡" + item.def : "";
                    string itemCount = itemId.Value > 1 ? " (" + itemId.Value + ")" : "";
                    itemList += "\n" + item.name + itemCount + stats + " /on_" + item.id;
                }
            }
            string equipList = "";
            if (equipment != null)
            {

                if (equipment.Any(e => e.Key == "slotRightHand" || e.Key == "slotBothHands"))
                {
                    var item = Items.GetItemById(int.Parse(equipment.FirstOrDefault(e => e.Key == "slotRightHand" || e.Key == "slotBothHands").Value.ToString()));
                    equipList += item.slot.Equals("slotRightHand") ? "\n<i>Правая рука:</i>\n" + "<b>" + item.name + "</b>" : "\n<i>Обе руки:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
                if (equipment.Keys.Contains("slotLeftHand"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotLeftHand"].ToString()));
                    equipList += "\n<i>Левая рука:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
                if (equipment.Keys.Contains("slotFace"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotFace"].ToString()));
                    equipList += "\n<i>Лицо:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
                if (equipment.Keys.Contains("slotHead"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotHead"].ToString()));
                    equipList += "\n<i>Голова:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
                if (equipment.Keys.Contains("slotChest"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotChest"].ToString()));
                    equipList += "\n<i>Тело:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
                if (equipment.Keys.Contains("slotBack"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotBack"].ToString()));
                    equipList += "\n<i>Плащ:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
                if (equipment.Keys.Contains("slotHands"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotHands"].ToString()));
                    equipList += "\n<i>Плечи:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
                if (equipment.Keys.Contains("slotArms"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotArms"].ToString()));
                    equipList += "\n<i>Перчатки:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
                if (equipment.Keys.Contains("slotLegs"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotLegs"].ToString()));
                    equipList += "\n<i>Ноги:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
                if (equipment.Keys.Contains("slotFeet"))
                {
                    var item = Items.GetItemById(int.Parse(equipment["slotFeet"].ToString()));
                    equipList += "\n<i>Ботинки:</i>\n" + "<b>" + item.name + "</b>";
                    equipList += item.atk > 0 ? " ⚔" + item.atk : "";
                    equipList += item.def > 0 ? " 🛡" + item.def : "";
                    equipList += " /off_" + item.id;
                }
            }
                await SendMessage("<b>Экипировка</b>\n<b>----------------------------</b>\n"+equipList+ "\n<b>----------------------------</b>\n<b>Рюкзак</b>\n<b>----------------------------</b>\n"+itemList);
        }

        public async void SetTitle(int titleId)
        {
            if (titleId == 0)
            {
                title = "";
                await SendMessage("Титулы и звания скрыты.");
            }

            if (titleId !=0 && achievements.Contains(titleId))
            {
                title = Achievements.GetAchById(titleId).name;
                await SendMessage("Теперь вы известны как <b>" + title + " " + name + "</b>!");
            }
            
            if (titleId !=0 && !achievements.Contains(titleId))
            {
                await SendMessage("Этот титул тебе не принадлежит, жалкий самозванец");
            }
        }

        public async void Die()
        {
            var lostExp = GetExp() / 3;
            var lostGold = gold / 10;
            exp = exp - lostExp;
            gold = gold - lostGold;
            await SendMessage("Вы умерли и потеряли " + lostExp + " опыта и " + lostGold + " золота", MainPage.GetKeyboard());
            await Task.Delay(10000);
            hp = 1;
            Persist();
        }
    }
}
