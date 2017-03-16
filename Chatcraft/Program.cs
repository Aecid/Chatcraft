using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Chatcraft
{
    public class Program
    {
        static TelegramBotClient Bot = BotClient.Instance;
        static SessionStorage sessions = new SessionStorage();
        static DateTime BotStartTime = DateTime.Now;
        static List<long> admins;

        public static void Main(string[] args)
        {
            admins = new List<long>();
            

            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            var timer = new System.Threading.Timer(
                e => sessions.RegenNotInQuest(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Превед!");
            string text = System.IO.File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\chars\\test.json");
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            ////Debugger.Break();
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Player currentSession;

            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.TextMessage ) return;
            string username;
            username = message.Chat.Username == null ? "UnnamedPlayer" : message.Chat.Username;
            currentSession = sessions.GetSession(message.Chat.Id, username);

            Console.WriteLine("{0} Message by username: {1}, chatId: {2}, message body: {3}", DateTime.Now.ToString("HH:mm:ss tt"), username, message.Chat.Id, message.Text);

            if (message.Text.StartsWith("/broadcast"))
            {
                if (admins.Contains(message.Chat.Id))
                {
                    var txt = message.Text;
                    if (txt.Contains(' '))
                    {
                        string msg = txt.Substring(txt.IndexOf(' ') + 1);
                        sessions.Broadcast(msg);
                    }
                }
            }

            if (message.Text.StartsWith("/start"))
            {
                if (currentSession.Name == null)
                {
                    await currentSession.SendMessage("Как тебя зовут?");
                }
                else
                {
                    await currentSession.SendMessage("Привет, " + currentSession.Name, MainPage.GetKeyboard());
                }
            }

            if (!message.Text.StartsWith("/start") && currentSession.Name == null)
            {
                currentSession.Name = message.Text;
                await currentSession.SendMessage("Ваш пол?" + currentSession.Name, Helper.GetKeyboard(new string[] { "♂ Мужской", "♀ Женский" }));
                currentSession.Persist();
            }

            if (message.Text.StartsWith("♂ Мужской"))
            {
                currentSession.Gender = true;
                await currentSession.SendMessage("Приветствую, сэр " + currentSession.Name, MainPage.GetKeyboard());
                currentSession.Persist();
            }

            if (message.Text.StartsWith("♀ Женский"))
            {
                currentSession.Gender = false;
                await currentSession.SendMessage("Приветствую, леди " + currentSession.Name, MainPage.GetKeyboard());
                currentSession.Persist();
            }

            if (message.Text.StartsWith("Приключения"))
            {
                if (!currentSession.InQuest)
                {
                    currentSession.SendInlineMessage("Вы видите огромный столб, а на нём куча указателей.", QuestsPage.GetKeyboard());
                }
                else
                {
                    await currentSession.SendMessage("Вы уже на задании.");
                }
            }

            if (message.Text.StartsWith("Инвентарь"))
            {
                currentSession.ShowBackpack();
            }

            if (!currentSession.InQuest)
            {
                if (message.Text.StartsWith("Магазин"))
                {
                    await currentSession.SendMessage("Здоровенный бугай смотрит на вас с ленивым интересом.\n-Будешь покупать что, или так просто зенки пыришь?", Helper.GetKeyboard(new string[] { "Купить", "Продать" }, new string[] { "Назад" }));
                }

                if (message.Text.StartsWith("Купить"))
                {
                    Shop.BuyItems(currentSession);
                }

                if (message.Text.StartsWith("Оружие"))
                {
                    Shop.BuyItems(currentSession);
                }

                if (message.Text.StartsWith("Броня"))
                {
                    Shop.BuyItems(currentSession);
                }

                if (message.Text.StartsWith("Одноручное оружие"))
                {
                    Shop.BuyItems(currentSession);
                }

                if (message.Text.StartsWith("Двуручное оружие"))
                {
                    Shop.BuyItems(currentSession);
                }

                if (message.Text.StartsWith("Шлемы"))
                {
                    Shop.BuyItems(currentSession);
                }

                if (message.Text.StartsWith("Шлемы"))
                {
                    Shop.BuyItems(currentSession);
                }

                if (message.Text.StartsWith("Продать"))
                {
                    Shop.SellItems(currentSession);
                }
            }

            if (message.Text.StartsWith("Статус"))
            {
                //var watch = System.Diagnostics.Stopwatch.StartNew();
                ////var msg = await BotClient.Instance.SendTextMessageAsync(currentSession.id, "Статус", replyMarkup: MainPage.GetKeyboard(), parseMode: ParseMode.Html);
                ////await BotClient.Instance.EditMessageReplyMarkupAsync(currentSession.id, msg.MessageId, currentSession.GetStatusKeyboard());
                currentSession.SendInlineMessage(currentSession.GetFullStatus(), currentSession.GetStatusKeyboard());
                //watch.Stop();
                //var elapsedMs = watch.ElapsedMilliseconds;
                //Console.WriteLine("Get full status, {0}, {1}: {2}", currentSession.id, currentSession.name, elapsedMs);
            }

            if (message.Text.StartsWith("/info"))
            {
                var txt = message.Text;
                if (txt.Contains('_'))
                {

                    var id = int.Parse(txt.Split('_')[1]);
                    await currentSession.SendMessage(Items.GetItemInfo(id));
                }
            }

            if (message.Text.StartsWith("/on"))
            {
                var txt = message.Text;
                if (txt.Contains('_'))
                {

                    var id = int.Parse(txt.Split('_')[1]);
                    currentSession.EquipItem(id);
                }
            }

            if (message.Text.StartsWith("/off"))
            {
                var txt = message.Text;
                if (txt.Contains('_'))
                {
                    var id = int.Parse(txt.Split('_')[1]);
                    currentSession.UnequipItem(id);
                }
            }

            if (message.Text.StartsWith("/buy"))
            {
                var txt = message.Text;
                if (txt.Contains('_'))
                {
                    var id = int.Parse(txt.Split('_')[1]);
                    currentSession.BuyItem(id);
                }
            }

            if (message.Text.StartsWith("/sell"))
            {
                var txt = message.Text;
                if (txt.Contains('_'))
                {
                    var id = int.Parse(txt.Split('_')[1]);
                    currentSession.SellItem(id);
                }
            }

            if (message.Text.StartsWith("/setTitle"))
            {
                var txt = message.Text;
                if (txt.Contains('_'))
                {
                    var id = int.Parse(txt.Split('_')[1]);
                    currentSession.SetTitle(id);
                }
            }

            if (message.Text.StartsWith("/levelUp"))
            {
                if (currentSession.AttributePoints != 0)
                    await currentSession.SendMessage("У вас [" + currentSession.AttributePoints + "] свободных очков характеристик.\nКакую характеристику вы хотите улучшить?", Helper.GetKeyboard(new string[][] { new string[] { "+1 💪Сила", "+1 🎯Ловкость" }, new string[] { "+1 🚜Выносливость" }, new string[] { "Назад" } }));
            }

            if (message.Text.StartsWith("+1"))
            {
                var txt = message.Text;
                if (txt.Contains(' '))
                {
                    var attr = txt.Split(' ')[1];
                    currentSession.AddAttribute(attr);
                }
            }

            if (message.Text.StartsWith("/name"))
            {
                var txt = message.Text;
                if (txt.Contains(' '))
                {
                    var name = txt.Split(' ')[1];
                    currentSession.SetName(name);
                }
            }

            if (message.Text.StartsWith("/gender"))
            {
                await currentSession.SendMessage("Ваш пол?", Helper.GetKeyboard(new string[] { "♂ Мужской", "♀ Женский" }));
            }

            if (message.Text.StartsWith("Назад") || message.Text.StartsWith("/back"))
            {
                await currentSession.SendMessage(currentSession.GetStatus(), MainPage.GetKeyboard());
            }

            if (message.Text.StartsWith("/who"))
            {
                var txt = message.Text;

                if (txt.Contains(' '))
                {
                    var name = txt.Split(' ')[1];
                    await currentSession.SendMessage(sessions.GetPlayerByName(name, currentSession).GetAlienStatus());
                }

            }


        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            Player currentSession;

            var callbackQueryData = callbackQueryEventArgs;
            if (callbackQueryData == null || callbackQueryData.CallbackQuery.Message.Type != MessageType.TextMessage) return;
            string username;
            username = callbackQueryData.CallbackQuery.Message.Chat.Username == null ? "UnnamedPlayer" : callbackQueryData.CallbackQuery.Message.Chat.Username;
            currentSession = sessions.GetSession(callbackQueryData.CallbackQuery.Message.Chat.Id, username);

            var task = callbackQueryEventArgs.CallbackQuery.Data;
            try
            {
                await BotClient.Instance.EditMessageTextAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, callbackQueryEventArgs.CallbackQuery.Message.MessageId, task);
            }
            catch (Exception e)
            { }

            if (currentSession == null)
            {
                username = callbackQueryEventArgs.CallbackQuery.From.Username == null ? "UnnamedPlayer" : callbackQueryEventArgs.CallbackQuery.From.Username;
                currentSession = sessions.GetSession(callbackQueryEventArgs.CallbackQuery.From.Id, username);

                if (currentSession.Name == null)
                {
                    await currentSession.SendMessage("Как тебя зовут?");
                }
                else
                {
                    await currentSession.SendMessage("Привет, " + currentSession.Name, MainPage.GetKeyboard());
                }
            }
            else
            {
                switch (task)
                {
                    case "Тёмный Лес ⬆":
                        currentSession.StartQuest("Лес");
                        break;
                    case "Пещера ⬅":
                        currentSession.StartQuest("Пещера");
                        break;
                    case StringConstants.OLD_Castle:
                        currentSession.StartQuest(StringConstants.OLD_Castle);
                        break;
                    //case "Шахта ➡":
                    //    currentSession.StartQuest("Лес");
                    //    break;
                    //case "Заброшенный город ↙":
                    //    currentSession.StartQuest("Лес");
                    //    break;
                    case "Назад ⬇":
                        currentSession.SendMessage(currentSession.GetStatus());
                        break;
                    case "Опции":
                        currentSession.ShowOptions();
                        break;
                    case "Рюкзак":
                        currentSession.ShowBackpack();
                        break;
                    case "Достижения":
                        currentSession.ShowAchievements();
                        break;
                    case "Статистика":
                        currentSession.ShowStats();
                        break;
                    default:
                        await currentSession.SendMessage("🚧 404. Ундер конструкшын 🚧", MainPage.GetKeyboard());
                        break;

                }
            }
        }
    }
}
