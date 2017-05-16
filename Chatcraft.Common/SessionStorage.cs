using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    public class SessionStorage
    {
        /// <summary>
        /// Словарь игроков
        /// </summary>
        private Dictionary<long,Player> chatIdToPlayersDict;
        string dir;

        public SessionStorage()
        {
            chatIdToPlayersDict = new Dictionary<long, Player>();
            dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public Player GetSession(long chatId, string username = "")
        {
            Player pl = null;
            if (chatIdToPlayersDict.TryGetValue(chatId, out pl))
                return chatIdToPlayersDict[chatId];
            else
            {
                if (File.Exists(dir + "//chars//" + chatId + ".json"))
                {
                    var currentSession = DeserializeSession(chatId.ToString());
                    chatIdToPlayersDict.Add(chatId,currentSession);
                    return currentSession;
                }
                else
                {
                    var currentSession = new Player();
                    currentSession.Update();
                    currentSession.Id = chatId;
                    currentSession.Username = username.Equals(string.Empty) ? "UnnamedPlayer" : username;
                    currentSession.Name = username;
                    currentSession.PageId = "MainPage";
                    chatIdToPlayersDict.Add(chatId,currentSession);
                    return currentSession;
                }
            }            
        }
        /// <summary>
        /// Получить словарь игроков
        /// </summary>
        /// <returns></returns>
        public Dictionary<long,Player> GetPlayersSessions()
        {
            return chatIdToPlayersDict;
        }
        /// <summary>
        /// Сериализация сессии игрока ( отдельный файл для каждого игрока)
        /// </summary>
        /// <param name="session"></param>
        public void SerializeSession(Player session)
        {
            Helper.WriteToJsonFile(dir + "//chars//" + session.Id + ".json", session);
        }

        public Player DeserializeSession(string sessionId)
        {
            return Helper.ReadFromJsonFile<Player>(dir + "//chars//" + sessionId + ".json");
        }

        public Player GetPlayerByName(string name, Player player)
        {
            Player pl = null;
            foreach(var chatId in chatIdToPlayersDict.Keys)
            {
                if(chatIdToPlayersDict[chatId].Name==name)
                    return chatIdToPlayersDict[chatId];
            }
            return player;            
        }

        public void Broadcast(string message, bool info = true)
        {
            message = "<b>" + message + "</b>";
            if (info) message = "⚠ " + message;
            foreach (var chatId in chatIdToPlayersDict.Keys)
            {
               chatIdToPlayersDict[chatId].SendMessage(message);
            }
        }
        /// <summary>
        /// Восстановить здоровье , если игрок не проходит квест
        /// </summary>
        public void RegenNotInQuest()
        {
            foreach(var player in chatIdToPlayersDict.Where(x => x.Value.GetInQuest() == false))
            {
                player.Value.Heal(10);
                player.Value.Mana(10);
            }            
        }

        /// <summary>
        /// Начать драку (не сделано)
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        public async void StartCombat(Player player1, Player player2)
        {


            player1.SetInQuest(true);
            player2.SetInQuest(true);
            Stopwatch s = new Stopwatch();
            s.Start();
            int i = 0;
            while (s.Elapsed < TimeSpan.FromSeconds(30))
            {
                ////await Task.Delay(100000);
                ////++i;
                ////session.SendMessage("Tick #" + i + " Time:" + s.Elapsed);
            }
            s.Stop();
        }
    }
}
