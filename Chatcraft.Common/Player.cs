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
    /// <summary>
    /// class of Player
    /// </summary>
    public class Player
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
        public long Gold { get; set; }
        public long Hp { get; set; }
        public long MaxHP { get; set; }
        public long Mp { get; set; }
        public long MaxMP { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intellect { get; set; }
        public int Constitution { get; set; }
        public int Charisma { get; set; }
        public int Luck { get; set; }
        public List<int> Items { get; set; }
        public int AttributePoints { get; set; }
        public List<int> Achievements { get; set; }
        public SortedDictionary<string, int> Statistic { get; set; }
        /// <summary>
        /// Equipments
        /// </summary>
        public Dictionary<string, object> Equipment { get; set; }
        public bool Gender { get; set; }
        public int Guild;
        /// <summary>
        /// Последний интервал времени активности игрока
        /// </summary>
        private TimeSpan _lastTimeSpan;
        public Player()
        {
            InQuest = false;
            Exp = 0;
            Level = 1;
            Gold = 0;
            Hp = 10;

            Strength = 1;
            Dexterity = 1;
            Intellect = 1;
            Constitution = 1;
            Charisma = 0;
            Luck = 0;

            MaxHP = Constitution * 10;
            MaxMP = Intellect * 10;

            Items = new List<int>();
            Statistic = new SortedDictionary<string, int>();
            Equipment = new Dictionary<string, object>();
            Achievements = new List<int>();
            _lastTimeSpan = new TimeSpan(DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute);
        }
        /// <summary>
        /// get level
        /// </summary>
        /// <returns></returns>
        public int GetLevel()
        {
            return Level;
        }
        /// <summary>
        /// Get hp
        /// </summary>
        /// <returns></returns>
        public long GetHP()
        {
            var char_maxHP = GetMaxHP();
            if (!NeedToResporeHp())
            {
                Hp = Hp > char_maxHP ? char_maxHP : Hp;
            }
            else
            {
                Hp = char_maxHP;                
            }
            return Hp;
        }
        /// <summary>
        /// Нужно ли восстановить игроку HP? (5 минут по-умолчанию)
        /// </summary>
        /// <returns></returns>
        public bool NeedToResporeHp(int period=5)
        {
            var currentTs = new TimeSpan(DateTime.UtcNow.Ticks);

            if (currentTs.TotalMinutes-_lastTimeSpan.TotalMinutes>period)// если прошло больше чем 5 минут,то true
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// get mana points
        /// </summary>
        /// <returns></returns>
        public long GetMP()
        {
            var char_maxMP = GetMaxMP();
            Mp = Mp > char_maxMP ? char_maxMP : Mp;
            return Mp;
        }
        /// <summary>
        /// get maxinum hp
        /// </summary>
        /// <returns></returns>
        public long GetMaxHP()
        {
            var char_maxHP = GetConst() * 10;
            Hp = Hp > char_maxHP ? char_maxHP : Hp;
            return char_maxHP;
        }
        /// <summary>
        /// get maximum mana points
        /// </summary>
        /// <returns></returns>
        public long GetMaxMP()
        {
            var char_maxMP = GetIntellect() * 10;
            Mp = Mp > char_maxMP ? char_maxMP : Mp;
            return char_maxMP;
        }
        /// <summary>
        /// get strength
        /// </summary>
        /// <returns></returns>
        public int GetStrength()
        {
            var str = Strength;
            foreach (var equip in Equipment)
            {
                str += Chatcraft.Items.GetItemById(int.Parse(equip.Value.ToString())).ModStr;
            }
            return str;
        }
        /// <summary>
        /// Узнать интеллект героя
        /// </summary>
        /// <returns></returns>
        public int GetIntellect()
        {
            var inta = Intellect;
            foreach (var equip in Equipment)
            {
                inta += Chatcraft.Items.GetItemById(int.Parse(equip.Value.ToString())).ModInt;
            }
            return inta;
        }

        public int GetConst()
        {
            var cons = Constitution;
            foreach (var equip in Equipment)
            {
                cons += Chatcraft.Items.GetItemById(int.Parse(equip.Value.ToString())).ModCon;
            }
            return cons;
        }

        public int GetDex()
        {
            var dex = Dexterity;
            foreach (var equip in Equipment)
            {
                dex += Chatcraft.Items.GetItemById(int.Parse(equip.Value.ToString())).ModDex;
            }
            return dex;
        }
        /// <summary>
        /// Узнать уровень харизмы
        /// </summary>
        /// <returns></returns>
        public int GetCharisma()
        {
            var cha = Charisma;
            foreach (var equip in Equipment)
            {
                cha += Chatcraft.Items.GetItemById(int.Parse(equip.Value.ToString())).ModCha;
            }
            return cha;
        }
        /// <summary>
        /// Узнать уровень удачи героя
        /// </summary>
        /// <returns></returns>
        public int GetLuck()
        {
            var _luck = Luck;
            foreach (var equip in Equipment)
            {
                _luck += Chatcraft.Items.GetItemById(int.Parse(equip.Value.ToString())).ModLuck;
            }
            return _luck;
        }

        /// <summary>
        /// get expierence of player
        /// </summary>
        /// <returns></returns>
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
                double aa = x + 300 * Math.Pow(2, x / 17.0);
                num += Math.Floor(aa);
            }
            return (int)Math.Floor(num / 4);
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

            var left = new Dictionary<string, string>()
            {
                { "Имя", GetTitle() + " " + Name },
                { "Здоровье", GetHP()+"/"+GetMaxHP() },
                //{ "Мана", GetMP()+"/"+GetMaxMP() },
                { "Уровень", GetLevel().ToString() },
                { "Опыт", GetExp()+"/"+GetExpTNL() },
                { "Золото", Gold.ToString() },
                { "aaa", "%divider%" },
                { "Урон", GetAttackString().Replace("⚔", "") },
                { "Защита", GetDefenseString().Replace("🛡", "") },
                { "bbb", "%divider%"},
                { "Сила", GetStrength().ToString() },
                { "Ловкость", GetDex().ToString()},
                //{ "Интеллект", GetInt().ToString()},
                { "Выносливость", GetConst().ToString()},
                { "Харизма", GetCharisma().ToString() },
                { "Удача", GetLuck().ToString() }
            };

            var right = new Dictionary<string, string>()
            {
                { "Сила", GetStrength().ToString() },
                { "Ловкость", GetDex().ToString()},
                //{ "Интеллект", GetInt().ToString()},
                { "Выносливость", GetConst().ToString()},
                { "Харизма", GetCharisma().ToString() },
                { "Удача", GetLuck().ToString() }
            };

            return "<pre>" + Helper.FormattedTable(left) + "</pre>";
            ///return string.Format("⭐️({0}) {1} {2}|♥️{3}/{4}|🔮{5}/{6}🔥{7}|💰{8}|", GetLevel(), GetTitle(), name, GetHP(), GetMaxHP(), GetMP(), GetMaxMP(), exp, gold);
        }

        public string GetAlienStatus()
        {
            var equipSb = new StringBuilder();            
            foreach (var item in Equipment)
            {
                equipSb.Append(Chatcraft.Items.GetItemName((int)item.Value));
                equipSb.Append("|");
            }
            return string.Format("⭐️({0}) {1} {2}|♥️{3}|🔮{4}|{5}", GetLevel(), GetTitle(), Name, GetMaxHP(), GetMaxMP(), equipSb);
        }
        /// <summary>
        /// Get player state
        /// </summary>
        /// <returns></returns>
        public string GetState()
        {
            return "Баклуши бьёт";
        }

        /// <summary>
        /// Get full status of player
        /// </summary>
        /// <returns></returns>
        public string GetFullStatus()
        {
            Persist();
            var itemList = new StringBuilder();

            foreach (var itemId in Items.GroupBy(i => i).ToDictionary(s => s.Key, s => s.Count()))
            {
                var item = Chatcraft.Items.GetItemById(itemId.Key);

                var stats = new StringBuilder();
                stats.Append(item.Atk > 0 ? " ⚔" + item.Atk : String.Empty);
                stats.Append(item.Def > 0 ? " 🛡" + item.Def : String.Empty);

                var itemCount = new StringBuilder();
                itemCount.Append(itemId.Value > 1 ? " (" + itemId.Value + ")" : String.Empty);
                itemList.Append("\n");
                itemList.Append(item.Name);
                itemList.Append(itemCount);
                itemList.Append(stats);
                itemList.Append(" /on_");
                itemList.Append(item.Id);                
            }

            var equipList = new StringBuilder(5000);//try to guess length of string...

            if (Equipment.Any(e => e.Key == "slotRightHand" || e.Key == "slotBothHands"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment.FirstOrDefault((KeyValuePair<string, object> e) => e.Key == "slotRightHand" || e.Key == "slotBothHands").Value.ToString()));                

                equipList.Append(item.Slot.Equals("slotRightHand") ? String.Format("\n<i>Правая рука:</i>\n <b>{0}</b>", item.Name) 
                    : String.Format("\n<i>Обе руки:</i>\n<b>{0}</b>",item.Name));
                equipList.Append(item.Atk > 0 ? $" ⚔ {item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡 + {item.Def}" :String.Empty);
            }
            if (Equipment.Keys.Contains("slotLeftHand"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotLeftHand"].ToString()));
                equipList.Append($"\n<i>Левая рука:</i>\n<b>{item.Name}</b>");
                equipList.Append(item.Atk > 0 ? $" ⚔{item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡{item.Def}" : String.Empty);
            }
            if (Equipment.Keys.Contains("slotFace"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotFace"].ToString()));
                equipList.Append( $"\n<i>Лицо:</i>\n<b>{item.Name}</b>");
                equipList.Append(item.Atk > 0 ? $" ⚔{item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡{item.Def}" : String.Empty);
            }
            if (Equipment.Keys.Contains("slotHead"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotHead"].ToString()));
                equipList.Append($"\n<i>Голова:</i>\n<b>{item.Name}</b>");
                equipList.Append(item.Atk > 0 ? $" ⚔{item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡{item.Def}" : String.Empty);
            }
            if (Equipment.Keys.Contains("slotChest"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotChest"].ToString()));
                equipList.Append($"\n<i>Тело:</i>\n<b>{item.Name}</b>");
                equipList.Append(item.Atk > 0 ? $" ⚔{item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡{item.Def}" : String.Empty);
            }
            if (Equipment.Keys.Contains("slotBack"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotBack"].ToString()));
                equipList.Append($"\n<i>Плащ:</i>\n<b>{item.Name}</b>");
                equipList.Append(item.Atk > 0 ? $" ⚔{item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡{item.Def}" : String.Empty);
            }
            if (Equipment.Keys.Contains("slotHands"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotHands"].ToString()));
                equipList.Append($"\n<i>Плечи:</i>\n<b>{item.Name}</b>");
                equipList.Append(item.Atk > 0 ? $" ⚔{item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡" + item.Def : String.Empty);
            }
            if (Equipment.Keys.Contains("slotArms"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotArms"].ToString()));
                equipList.Append($"\n<i>Перчатки:</i>\n<b>{item.Name}</b>");
                equipList.Append(item.Atk > 0 ? $" ⚔{item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡{item.Def}" : String.Empty);
            }
            if (Equipment.Keys.Contains("slotLegs"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotLegs"].ToString()));
                equipList.Append($"\n<i>Ноги:</i>\n<b>{item.Name}</b>");
                equipList.Append(item.Atk > 0 ? $" ⚔{item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡{item.Def}" : String.Empty);
            }
            if (Equipment.Keys.Contains("slotFeet"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotFeet"].ToString()));
                equipList.Append($"\n<i>Ботинки:</i>\n<b>{item.Name}</b>");
                equipList.Append(item.Atk > 0 ? $" ⚔{item.Atk}" : String.Empty);
                equipList.Append(item.Def > 0 ? $" 🛡{item.Def}" : String.Empty);
            }
            string levelUp = "";
            if (AttributePoints > 0)
            {
                levelUp += "<b>----------------------------</b>\n<b>Свободные очки характеристик: [" + AttributePoints + "]\nДля повышения уровня используйте команду</b> /levelUp\n";
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
🎩<b>Экипировка:</b>{17}", "", GetTitle(), Name, GetHP(), GetMaxHP(), GetMP(), GetMaxMP(), GetLevel(), GetExp(), Gold, GetStrength(), GetDex(), GetIntellect(), GetConst(), GetCharisma(), GetLuck(), itemList, equipList, GetExpTNL(), levelUp, GetAttackString(), GetDefenseString(), Items.Count());
        }

        public async void SetName(string desiredName)
        {
            if (Name.Length > 2 && Name.Length <= 32 && Regex.IsMatch(desiredName, @"^[a-zA-Zа-яА-Я0-9]+$")) {
            Name = desiredName;
            Persist();
                string title = "Приветствую, ";
                    title += Gender ? "сэр " : "леди ";
                title += Name;
                await SendMessage(title);
            } else
            {
                await SendMessage("Имя должно быть от 3 до 32 символов в длину и не содержать специальных символов.");
            }
        }
        /// <summary>
        /// Показать доступные опции персонажа
        /// </summary>
        public async void ShowOptions()
        {
            string optionsMessage = @"Доступные опции:
Смена имени /name {имя} (Установленное имя персонажа: "+Name+@") 
Смена пола /gender";
            await SendMessage(optionsMessage);
        }
        /// <summary>
        /// Сохранить данные
        /// </summary>
        public void Persist()
        {
            Helper.WriteToJsonFile(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "//chars//" + Id + ".json", this);
        }

        /// <summary>
        /// Послать фото
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendPhoto(string photo, string message)
        {
            await BotClient.Instance.SendChatActionAsync(Id, ChatAction.UploadPhoto);

            string file = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "//img//" + photo;

            var fileName = file.Split('/').Last();

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

                string file = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "//img//" + photo;

                var fileName = file.Split('/').Last();

                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fts = new FileToSend(fileName, fileStream);

                    await BotClient.Instance.SendPhotoAsync(Id, fts, message, replyMarkup: keyboard);
                }
            }

            if (photo == null && keyboard != null)
            {
                await SendKeyboard(keyboard, message);
            }

            if (photo != null && keyboard == null)
            {
                await SendPhoto(photo, message);
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

                string file = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "//img//" + photo;

                var fileName = file.Split('/').Last();

                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fts = new FileToSend(fileName, fileStream);

                    await BotClient.Instance.SendPhotoAsync(Id, fts, message, replyMarkup: keyboard);
                }
            }

            if (photo == null && keyboard != null)
            {
                await SendInlineKeyboard(keyboard, message);
            }

            if (photo != null && keyboard == null)
            {
                await SendPhoto(photo, message);
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
        /// <summary>
        /// начать квест
        /// </summary>
        /// <param name="quest"></param>
        public void StartQuest(string quest)
        {
            if (!InQuest)
            {
                switch (quest)
                {
                    case StringConstants.FOREST:
                        ForestQuest();
                        break;
                    case StringConstants.CAVE:
                        CaveQuest();
                        break;
                    case StringConstants.OLD_Castle:
                        CastleQuest();
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

        public void CastleQuest()
        {
            QuestsPage.CastleQuest(this);
        }

        public void AddGold(long amount)
        {
            Gold += amount;
            Gold = Gold < 0 ? 0 : Gold;
        }

        public void AddExp(long amount)
        {
            Exp += amount;
            Exp = Exp < 0 ? 0 : Exp;
            if (Exp >= GetExpTNL())
            {
                var _exp = Exp - GetExpTNL();

                //levelup
                Level++;
                //constitution++;
                AttributePoints++;
                LevelUp();
                Exp = _exp;
            }
        }

        public void Heal(long amount)
        {
            Hp += amount;
            Hp = Hp > GetMaxHP() ? GetMaxHP() : Hp;
        }

        public void Mana(long amount)
        {
            Mp += amount;
            Mp = Mp > GetMaxMP()
                ? GetMaxMP()
                : Mp;
        }

        public void DealDamage(long amount)
        {
            Hp -= amount;
            Hp = Hp < 0 ? 0 : Hp;
            if (Hp <= 0)
            {
                Die();
            }
        }

        internal void AddItem(List<int> addItems)
        {
            foreach (var item in addItems)
            {
                if (Items == null)
                {
                    Items = new List<int>();
                }

                Items.Add(item);
            }
            Persist();
        }

        public void EquipItem(int id)
        {

            var item = Chatcraft.Items.GetItemById(id);
            Items.Remove(id);

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

            Persist();
        }

        public void UnequipItem(int id)
        {                       
            if (Chatcraft.Items.ItemsList.Any(i => i.Id == id))
            {
                var item = Chatcraft.Items.GetItemById(id);

                if (Equipment.Any((KeyValuePair<string, object> e) => int.Parse(e.Value.ToString()) == id))
                {
                    Items.Add(id);
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
            if (Items == null) Items = new List<int>();

            if (Equipment.Keys.Contains(slot))
            {
                var itemId = int.Parse(Equipment[slot].ToString());
                Items.Add(itemId);
                Equipment.Remove(slot);
                SendMessage("Снято: " + Chatcraft.Items.GetItemById(itemId).Name);
            }
            else
            {
                SendMessage("Предмет не найден");
            }
            Persist();
        }

        public void BuyItem(int id)
        {           
            if (Chatcraft.Items.ItemsList.Any(i => i.Id == id))
            {
                var item = Chatcraft.Items.GetItemById(id);

                if (Gold < item.Price)
                {
                    SendMessage("Недостаточно золота.");
                }

                else
                {
                    Gold -= item.Price;
                    Items.Add(id);
                    SendMessage("Вы купили " + item.Name);
                }
                Persist();
            }
        }

        public void SellItem(int id)
        {           
            if (Chatcraft.Items.ItemsList.Any(i => i.Id == id))
            {

                var item = Chatcraft.Items.GetItemById(id);

                if (Items.Contains(id))
                {
                    Gold += item.Price / 3;
                    Items.Remove(id);
                    SendMessage("Вы продали " + item.Name);
                }
                else
                {
                    SendMessage("У вас нет предмета " + item.Name);
                }
                Persist();
            }
        }
        /// <summary>
        /// Повышение уровня
        /// </summary>
        public void LevelUp()
        {
            SendMessage("🏆Поздравляем, вы получили новый уровень!🏆\nИспользуй /levelUp для повышения выбранной характеристики.");
        }

        public void AddAttribute(string attribute)
        {
            if (AttributePoints > 0)
            {
                switch (attribute)
                {
                    case "💪Сила":
                        Strength++;
                        AttributePoints--;
                        string msg = "Добавлено +1 очко к " + attribute;
                        msg += AttributePoints > 0 ? "\nУ вас [" + AttributePoints + "] свободных очков характеристик" : "";
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
                        Dexterity++;
                        AttributePoints--;
                        msg = "Добавлено +1 очко к " + attribute;
                        msg += AttributePoints > 0 ? "\nУ вас [" + AttributePoints + "] свободных очков характеристик" : "";
                        SendMessage(msg);
                        break;
                    case "🚜Выносливость":
                        Constitution++;
                        AttributePoints--;
                        msg = "Добавлено +1 очко к " + attribute;
                        msg += AttributePoints > 0 ? "\nУ вас [" + AttributePoints + "] свободных очков характеристик" : "";
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
            int atk =  GetStrength() / 2;
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
                    var item = Chatcraft.Items.GetItemById(int.Parse(equippedItem.Value.ToString()));
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
                    var item = Chatcraft.Items.GetItemById(int.Parse(equippedItem.Value.ToString()));
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
            if (Statistic.ContainsKey(stat))
            {
                Statistic[stat]++;
                if (Chatcraft.Achievements.AchList.Any(a => a.Stat.Equals(stat)))
                {
                    if (Achievements == null) Achievements = new List<int>();
                    var achi = Chatcraft.Achievements.GetAchByStatName(stat);
                    if (!Achievements.Contains(achi.Id))
                    {
                        if (Statistic[stat] >= achi.Count)
                        {
                            Achievements.Add(achi.Id);
                            SendMessage("Новое достижение - 🏆<b>" + achi.Name + "</b>🏆!\n" + achi.Description);
                        }
                    }
                }
            }
            else
            {
                Statistic[stat] = 1;
            }
        }

        public InlineKeyboardMarkup GetStatusKeyboard()
        {
            List<string> options = new List<string>();
            //if (items.Count > 0) options.Add("Рюкзак");
            if (Achievements.Count > 0) options.Add("Достижения");
            if (Statistic.Count > 0) options.Add("Статистика");
            options.Add("Опции");
            return Helper.GetVerticalInlineKeyboardByList(options);
        }

        public async void ShowStats()
        {
            string result = "";
            
            foreach (var stat in Statistic)
            {
                result += "<i>" + stat.Key + ": " + stat.Value + "</i>\n";
            }

            await SendMessage(result);
        }

        public async void ShowAchievements()
        {
            string result = "";
            foreach (var achi in Achievements)
            {
                var ac = Chatcraft.Achievements.GetAchById(achi);
                result += "🏆" + ac.Name + ": " + ac.Description + " /setTitle_" + ac.Id + "\n";
            }
            result += "Убрать титул - /setTitle_0";
            await SendMessage(result);
        }

        public async void ShowBackpack()
        {
            string itemList = "";
            if (Items != null)
            {
                foreach (var itemId in Items.GroupBy(i => i).ToDictionary(s => s.Key, s => s.Count()))
                {
                    var item = Chatcraft.Items.GetItemById(itemId.Key);
                    var stats = "";
                    stats += item.Atk > 0 ? " ⚔" + item.Atk : "";
                    stats += item.Def > 0 ? " 🛡" + item.Def : "";
                    string itemCount = itemId.Value > 1 ? " (" + itemId.Value + ")" : "";
                    itemList += "\n" + item.Name + itemCount + stats + " /on_" + item.Id;
                }
            }
            string equipList = "";


            if (Equipment.Any(e => e.Key == "slotRightHand" || e.Key == "slotBothHands"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment.FirstOrDefault((KeyValuePair<string, object> e) => e.Key == "slotRightHand" || e.Key == "slotBothHands").Value.ToString()));
                equipList += item.Slot.Equals("slotRightHand") ? "\n<i>Правая рука:</i>\n" + "<b>" + item.Name + "</b>" : "\n<i>Обе руки:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }
            if (Equipment.Keys.Contains("slotLeftHand"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotLeftHand"].ToString()));
                equipList += "\n<i>Левая рука:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }
            if (Equipment.Keys.Contains("slotFace"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotFace"].ToString()));
                equipList += "\n<i>Лицо:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }
            if (Equipment.Keys.Contains("slotHead"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotHead"].ToString()));
                equipList += "\n<i>Голова:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }
            if (Equipment.Keys.Contains("slotChest"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotChest"].ToString()));
                equipList += "\n<i>Тело:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }
            if (Equipment.Keys.Contains("slotBack"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotBack"].ToString()));
                equipList += "\n<i>Плащ:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }
            if (Equipment.Keys.Contains("slotHands"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotHands"].ToString()));
                equipList += "\n<i>Плечи:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }
            if (Equipment.Keys.Contains("slotArms"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotArms"].ToString()));
                equipList += "\n<i>Перчатки:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }
            if (Equipment.Keys.Contains("slotLegs"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotLegs"].ToString()));
                equipList += "\n<i>Ноги:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }
            if (Equipment.Keys.Contains("slotFeet"))
            {
                var item = Chatcraft.Items.GetItemById(int.Parse(Equipment["slotFeet"].ToString()));
                equipList += "\n<i>Ботинки:</i>\n" + "<b>" + item.Name + "</b>";
                equipList += item.Atk > 0 ? " ⚔" + item.Atk : "";
                equipList += item.Def > 0 ? " 🛡" + item.Def : "";
                equipList += " /off_" + item.Id;
            }

            await SendMessage("<b>Экипировка</b>\n<b>----------------------------</b>\n" + equipList + "\n<b>----------------------------</b>\n<b>Рюкзак</b>\n<b>----------------------------</b>\n" + itemList);
        }

        public async void SetTitle(int titleId)
        {
            if (titleId == 0)
            {
                Title = "";
                await SendMessage("Титулы и звания скрыты.");
            }

            if (titleId !=0 && Achievements.Contains(titleId))
            {
                Title = Chatcraft.Achievements.GetAchById(titleId).Name;
                await SendMessage("Теперь вы известны как <b>" + Title + " " + Name + "</b>!");
            }
            
            if (titleId !=0 && !Achievements.Contains(titleId))
            {
                await SendMessage("Этот титул тебе не принадлежит, жалкий самозванец");
            }
        }

        public async void Die()
        {
            var lostExp = GetExp() / 3;
            var lostGold = Gold / 10;
            Exp -= lostExp;
            Gold -= lostGold;
            await SendMessage("Вы умерли и потеряли " + lostExp + " опыта и " + lostGold + " золота", MainPage.GetKeyboard());
            await Task.Delay(10000);
            Hp = 1;
            Persist();
        }
    }
}
