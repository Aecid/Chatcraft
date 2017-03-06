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
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Page id
        /// </summary>
        public string PageId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// is in Equest?
        /// </summary>
        public bool InQuest { get; set; }
        /// <summary>
        /// Experiense
        /// </summary>
        public long Exp { get; set; }
        /// <summary>
        /// Level
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Gold
        /// </summary>
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
        public Dictionary<string, object> Equipment { get; set; }
        public bool gender { get; set; }
        public int guild;

        public Session()
        {
            InQuest = false;
            Exp = 0;
            Level = 1;
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
            Equipment = new Dictionary<string, object>();
            achievements = new List<int>();
        }

        public int GetLevel()
        {
            GetExp();
            var char_level = Level;
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
            foreach (var equip in Equipment)
            {
                str += Items.GetItemById(int.Parse(equip.Value.ToString())).ModStr;
            }
            return str;
        }

        public int GetInt()
        {
            var inta = intellect;
            foreach (var equip in Equipment)
            {
                inta += Items.GetItemById(int.Parse(equip.Value.ToString())).ModInt;
            }
            return inta;
        }

        public int GetConst()
        {
            var cons = constitution;
            foreach (var equip in Equipment)
            {
                cons += Items.GetItemById(int.Parse(equip.Value.ToString())).ModCon;
            }
            return cons;
        }

        public int GetDex()
        {
            var dex = dexterity;
            foreach (var equip in Equipment)
            {
                dex += Items.GetItemById(int.Parse(equip.Value.ToString())).ModDex;
            }
            return dex;
        }

        public int GetCha()
        {
            var cha = charisma;
            foreach (var equip in Equipment)
            {
                cha += Items.GetItemById(int.Parse(equip.Value.ToString())).ModCha;
            }
            return cha;
        }

        public int GetLuck()
        {
            var _luck = luck;
            foreach (var equip in Equipment)
            {
                _luck += Items.GetItemById(int.Parse(equip.Value.ToString())).ModLuck;
            }
            return _luck;
        }

        public long GetExp()
        {
            return Exp;
        }

        public long GetExpTNL()
        {
            if (Level == 1) return 42;
            double num = 0;
            for (int x = 1; x < Level; x++)
            {
                double aa = (x + 300 * Math.Pow(2, (x / 17.0)));
                num += Math.Floor(aa);
            }
            int _exp = (int)Math.Floor(num / 4);

            return _exp;
        }

        public string GetTitle()
        {
            return Title;
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
                { "Имя", GetTitle() + " " + Name },
                { "Здоровье", GetHP()+"/"+GetMaxHP() },
                //{ "Мана", GetMP()+"/"+GetMaxMP() },
                { "Уровень", GetLevel().ToString() },
                { "Опыт", GetExp()+"/"+GetExpTNL() },
                { "Золото", gold.ToString() },
                { "aaa", "%divider%" },
                { "Урон", GetAttackString().Replace("⚔", "") },
                { "Защита", GetDefenseString().Replace("🛡", "") },
                { "bbb", "%divider%"},
                { "Сила", GetStr().ToString() },
                { "Ловкость", GetDex().ToString()},
                //{ "Интеллект", GetInt().ToString()},
                { "Выносливость", GetConst().ToString()},
                { "Харизма", GetCha().ToString() },
                { "Удача", GetLuck().ToString() }
            };

            Dictionary<string, string> right = new Dictionary<string, string>()
            {
                { "Сила", GetStr().ToString() },
                { "Ловкость", GetDex().ToString()},
                //{ "Интеллект", GetInt().ToString()},
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
            foreach (var item in Equipment)
            {
                equip += Items.GetItemName((int)item.Value) + "|";
            }
            return string.Format("⭐️({0}) {1} {2}|♥️{3}|🔮{4}|{5}", GetLevel(), GetTitle(), Name, GetMaxHP(), GetMaxMP(), equip);
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
                    stats += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    stats += item.Def > 0 ? " 🛡" + item.Def : "";
                    string itemCount = itemId.Value > 1 ? " (" + itemId.Value + ")" : ""; 
                    itemList += "\n" + item.Name + itemCount + stats + " /on_" + item.Id;
                }
            }
            string equipList = "";
            if (Equipment != null)
            {

                if (Equipment.Any(e => e.Key == "slotRightHand" || e.Key == "slotBothHands"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment.FirstOrDefault(e => e.Key == "slotRightHand" || e.Key == "slotBothHands").Value.ToString()));
                    equipList += item.Slot.Equals("slotRightHand") ? "\n<i>Правая рука:</i>\n" + "<b>" + item.Name + "</b>" : "\n<i>Обе руки:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                }
                if (Equipment.Keys.Contains("slotLeftHand"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotLeftHand"].ToString()));
                    equipList += "\n<i>Левая рука:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                }
                if (Equipment.Keys.Contains("slotFace"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotFace"].ToString()));
                    equipList += "\n<i>Лицо:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                }
                if (Equipment.Keys.Contains("slotHead"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotHead"].ToString()));
                    equipList += "\n<i>Голова:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                }
                if (Equipment.Keys.Contains("slotChest"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotChest"].ToString()));
                    equipList += "\n<i>Тело:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                }
                if (Equipment.Keys.Contains("slotBack"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotBack"].ToString()));
                    equipList += "\n<i>Плащ:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                }
                if (Equipment.Keys.Contains("slotHands"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotHands"].ToString()));
                    equipList += "\n<i>Плечи:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                }
                if (Equipment.Keys.Contains("slotArms"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotArms"].ToString()));
                    equipList += "\n<i>Перчатки:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                }
                if (Equipment.Keys.Contains("slotLegs"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotLegs"].ToString()));
                    equipList += "\n<i>Ноги:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                }
                if (Equipment.Keys.Contains("slotFeet"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotFeet"].ToString()));
                    equipList += "\n<i>Ботинки:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
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
⭐️Уровень: {7}
🔥Опыт: {8}/{18}
💰Золото: {9}
 <b>----------------------------</b>
👊<b>Характеристики:</b>
Атака {20}
Защита {21}
<b>----------------------------</b>
💪Сила: {10}
🎯Ловкость: {11}
🚜Выносливость: {13}
🎭Харизма: {14}
🎲Удача: {15}
{19}<b>----------------------------</b>
🎩<b>Экипировка:</b>{17}", "", GetTitle(), Name, GetHP(), GetMaxHP(), GetMP(), GetMaxMP(), GetLevel(), GetExp(), gold, GetStr(), GetDex(), GetInt(), GetConst(), GetCha(), GetLuck(), itemList, equipList, GetExpTNL(), levelUp, GetAttackString(), GetDefenseString(), items.Count());
        }

        internal async void SetName(string desiredName)
        {
            if (Name.Length > 2 && Name.Length <= 32 && Regex.IsMatch(desiredName, @"^[a-zA-Zа-яА-Я0-9]+$")) {
            Name = desiredName;
            Persist();
                string title = "Приветствую, ";
                    title += gender ? "сэр " : "леди ";
                title += Name;
                await SendMessage(title);
            } else
            {
                await SendMessage("Имя должно быть от 3 до 32 символов в длину и не содержать специальных символов.");
            }
        }

        internal async void ShowOptions()
        {
            string optionsMessage = @"Доступные опции:
Смена имени /name {имя} (Установленное имя персонажа: "+Name+@") 
Смена пола /gender";
            await SendMessage(optionsMessage);
        }

        public void Persist()
        {
            Helper.WriteToJsonFile(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\chars\\" + Id + ".json", this);
        }

        public async Task SendPhoto(string photo, string message)
        {
            await BotClient.Instance.SendChatActionAsync(Id, ChatAction.UploadPhoto);

            string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\img\\" + photo;

            var fileName = file.Split('\\').Last();

            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fts = new FileToSend(fileName, fileStream);

                await BotClient.Instance.SendPhotoAsync(Id, fts, message);
            }
        }

        public async Task SendKeyboard(ReplyKeyboardMarkup keyboard, string message)
        {
            await BotClient.Instance.SendTextMessageAsync(Id, message, replyMarkup: keyboard, parseMode: ParseMode.Html);
        }

        public async Task SendInlineKeyboard(InlineKeyboardMarkup keyboard, string message)
        {
            await BotClient.Instance.SendTextMessageAsync(Id, message, replyMarkup: keyboard, parseMode: ParseMode.Html);
        }

        public async Task SendMessage(string message, ReplyKeyboardMarkup keyboard = null, string photo = null)
        {
            if (photo != null && keyboard != null)
            {
                await BotClient.Instance.SendChatActionAsync(Id, ChatAction.UploadPhoto);

                string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\img\\" + photo;

                var fileName = file.Split('\\').Last();

                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fts = new FileToSend(fileName, fileStream);

                    await BotClient.Instance.SendPhotoAsync(Id, fts, message, replyMarkup: keyboard);
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
                    await BotClient.Instance.SendTextMessageAsync(Id, message, parseMode: ParseMode.Html);
                }
            }
        }

        public async void SendInlineMessage(string message, InlineKeyboardMarkup keyboard = null, string photo = null)
        {
            if (photo != null && keyboard != null)
            {
                await BotClient.Instance.SendChatActionAsync(Id, ChatAction.UploadPhoto);

                string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\img\\" + photo;

                var fileName = file.Split('\\').Last();

                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fts = new FileToSend(fileName, fileStream);

                    await BotClient.Instance.SendPhotoAsync(Id, fts, message, replyMarkup: keyboard);
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
                await BotClient.Instance.SendTextMessageAsync(Id, message);
            }
        }

        public void GotoPage(string _pageId)
        {
            PageId = _pageId;
        }

        public void StartQuest(string quest)
        {
            if (!InQuest)
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
            Exp = Exp + amount;
            Exp = Exp < 0 ? 0 : Exp;
            if (Exp >= GetExpTNL())
            {
                var _exp = Exp - GetExpTNL();

                //levelup
                Level++;
                //constitution++;
                attributePoints++;
                LevelUp();
                Exp = _exp;
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
            if (Equipment == null) Equipment = new Dictionary<string, object>();
            if (items != null && items.Contains(id))
            {
                var item = Items.GetItemById(id);
                items.Remove(id);

                switch (item.Slot)
                {
                    case "slotRightHand":
                        if (Equipment.ContainsKey(item.Slot))
                        {
                            UnequipItem(item.Slot);
                        }

                        if (Equipment.ContainsKey("slotBothHands"))
                        {
                            UnequipItem("slotBothHands");
                        }
                        Equipment[item.Slot] = id;
                        break;
                    case "slotLeftHand":
                        if (Equipment.ContainsKey(item.Slot))
                        {
                            UnequipItem(item.Slot);
                        }

                        if (Equipment.ContainsKey("slotBothHands"))
                        {
                            UnequipItem("slotBothHands");
                        }
                        Equipment[item.Slot] = id;
                        break;
                    case "slotBothHands":
                        if (Equipment.ContainsKey(item.Slot))
                        {
                            UnequipItem(item.Slot);
                        }

                        if (Equipment.ContainsKey("slotRightHand"))
                        {
                            UnequipItem("slotRightHand");
                        }

                        if (Equipment.ContainsKey("slotLeftHand"))
                        {
                            UnequipItem("slotLeftHand");
                        }
                        Equipment[item.Slot] = id;
                        break;
                    case "title":
                        break;
                    case "rings":
                        break;
                    default:
                        if (Equipment.Keys.Contains(item.Slot)) UnequipItem(int.Parse(Equipment[item.Slot].ToString()));
                        Equipment[item.Slot] = id;
                        break;
                }

                SendMessage("Экипировано: " + item.Name);
            }

            else
            {
                SendMessage("Предмет не найден");
            }
            Persist();
        }

        public void UnequipItem(int id)
        {
            if (Equipment == null) Equipment = new Dictionary<string, object>();
            if (items == null) items = new List<int>();
            if (Items.ItemsList.Any(i => i.Id == id))
            {
                var item = Items.GetItemById(id);

                if (Equipment.Any(e => int.Parse(e.Value.ToString()) == id))
                {
                    items.Add(id);
                    Equipment.Remove(item.Slot);
                    SendMessage("Снято: " + item.Name);
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
            if (Equipment == null) Equipment = new Dictionary<string, object>();
            if (items == null) items = new List<int>();

            if (Equipment.Keys.Contains(slot))
            {
                var itemId = int.Parse(Equipment[slot].ToString());
                items.Add(itemId);
                Equipment.Remove(slot);
                SendMessage("Снято: " + Items.GetItemById(itemId).Name);
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
            if (Items.ItemsList.Any(i => i.Id == id))
            {
                var item = Items.GetItemById(id);

                if (gold < item.Price)
                {
                    SendMessage("Недостаточно золота.");
                }

                else
                {
                    gold -= item.Price;
                    items.Add(id);
                    SendMessage("Вы купили " + item.Name);
                }
                Persist();
            }
        }

        public void SellItem(int id)
        {
            if (items == null) items = new List<int>();
            if (Items.ItemsList.Any(i => i.Id == id))
            {

                var item = Items.GetItemById(id);

                if (items.Contains(id))
                {
                    gold += item.Price / 3;
                    items.Remove(id);
                    SendMessage("Вы продали " + item.Name);
                }
                else
                {
                    SendMessage("У вас нет предмета " + item.Name);
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
                    //case "📖Интеллект":
                    //    intellect++;
                    //    attributePoints--;
                    //    msg = "Добавлено +1 очко к " + attribute;
                    //    msg += attributePoints > 0 ? "\nУ вас [" + attributePoints + "] свободных очков характеристик" : "";
                    //    SendMessage(msg);
                    //    break;
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
            foreach (var equippedItem in Equipment)
            {
                if (!equippedItem.Key.Equals("rings"))
                {
                    var item = Items.GetItemById(int.Parse(equippedItem.Value.ToString()));
                    attack += item.Atk;
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
            foreach (var equippedItem in Equipment)
            {
                if (!equippedItem.Key.Equals("rings"))
                {
                    var item = Items.GetItemById(int.Parse(equippedItem.Value.ToString()));
                    defense += item.Def;
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
                if (Achievements.AchList.Any(a => a.Stat.Equals(stat)))
                {
                    if (achievements == null) achievements = new List<int>();
                    var achi = Achievements.GetAchByStatName(stat);
                    if (!achievements.Contains(achi.Id))
                    {
                        if (statistic[stat] >= achi.Count)
                        {
                            achievements.Add(achi.Id);
                            SendMessage("Новое достижение - 🏆<b>" + achi.Name + "</b>🏆!\n" + achi.Description);
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
                result += "🏆" + ac.Name + ": " + ac.Description + " /setTitle_" + ac.Id + "\n";
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
                    stats += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    stats += item.Def > 0 ? " 🛡" + item.Def : "";
                    string itemCount = itemId.Value > 1 ? " (" + itemId.Value + ")" : "";
                    itemList += "\n" + item.Name + itemCount + stats + " /on_" + item.Id;
                }
            }
            string equipList = "";
            if (Equipment != null)
            {

                if (Equipment.Any(e => e.Key == "slotRightHand" || e.Key == "slotBothHands"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment.FirstOrDefault(e => e.Key == "slotRightHand" || e.Key == "slotBothHands").Value.ToString()));
                    equipList += item.Slot.Equals("slotRightHand") ? "\n<i>Правая рука:</i>\n" + "<b>" + item.Name + "</b>" : "\n<i>Обе руки:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
                if (Equipment.Keys.Contains("slotLeftHand"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotLeftHand"].ToString()));
                    equipList += "\n<i>Левая рука:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
                if (Equipment.Keys.Contains("slotFace"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotFace"].ToString()));
                    equipList += "\n<i>Лицо:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
                if (Equipment.Keys.Contains("slotHead"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotHead"].ToString()));
                    equipList += "\n<i>Голова:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
                if (Equipment.Keys.Contains("slotChest"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotChest"].ToString()));
                    equipList += "\n<i>Тело:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
                if (Equipment.Keys.Contains("slotBack"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotBack"].ToString()));
                    equipList += "\n<i>Плащ:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
                if (Equipment.Keys.Contains("slotHands"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotHands"].ToString()));
                    equipList += "\n<i>Плечи:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
                if (Equipment.Keys.Contains("slotArms"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotArms"].ToString()));
                    equipList += "\n<i>Перчатки:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
                if (Equipment.Keys.Contains("slotLegs"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotLegs"].ToString()));
                    equipList += "\n<i>Ноги:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
                if (Equipment.Keys.Contains("slotFeet"))
                {
                    var item = Items.GetItemById(int.Parse(Equipment["slotFeet"].ToString()));
                    equipList += "\n<i>Ботинки:</i>\n" + "<b>" + item.Name + "</b>";
                    equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                    equipList += " /off_" + item.Id;
                }
            }
                await SendMessage("<b>Экипировка</b>\n<b>----------------------------</b>\n"+equipList+ "\n<b>----------------------------</b>\n<b>Рюкзак</b>\n<b>----------------------------</b>\n"+itemList);
        }

        public async void SetTitle(int titleId)
        {
            if (titleId == 0)
            {
                Title = "";
                await SendMessage("Титулы и звания скрыты.");
            }

            if (titleId !=0 && achievements.Contains(titleId))
            {
                Title = Achievements.GetAchById(titleId).Name;
                await SendMessage("Теперь вы известны как <b>" + Title + " " + Name + "</b>!");
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
            Exp = Exp - lostExp;
            gold = gold - lostGold;
            await SendMessage("Вы умерли и потеряли " + lostExp + " опыта и " + lostGold + " золота", MainPage.GetKeyboard());
            await Task.Delay(10000);
            hp = 1;
            Persist();
        }
    }
}
