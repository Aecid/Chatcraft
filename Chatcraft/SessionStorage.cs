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
        private List<Player> sessions;
        string dir;

        public SessionStorage()
        {
            sessions = new List<Player>();
            dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public Player GetSession(long chatId, string username = "")
        {
            if (sessions.Exists(s => s.Id == chatId))
            {
                return sessions.FirstOrDefault(s => s.Id == chatId);
            }
            else
            {
                if (File.Exists(dir + "\\chars\\" + chatId + ".json"))
                {
                    var currentSession = DeserializeSession(chatId.ToString());
                    sessions.Add(currentSession);
                    return currentSession;
                }
                else
                {
                    var currentSession = new Player();
                    currentSession.Id = chatId;
                    currentSession.Username = username.Equals(string.Empty)?"UnnamedPlayer":username;
                    currentSession.Name = username;
                    currentSession.PageId = "MainPage";
                    sessions.Add(currentSession);
                    return currentSession;
                }
            }
        }

        public List<Player> GetSessions()
        {
            return sessions;
        }
        /// <summary>
        /// Сериализация сессии игрока ( отдельный файл для каждого игрока)
        /// </summary>
        /// <param name="session"></param>
        public void SerializeSession(Player session)
        {
            Helper.WriteToJsonFile(dir + "\\chars\\" + session.Id + ".json", session);
        }

        public Player DeserializeSession(string sessionId)
        {
            return Helper.ReadFromJsonFile<Player>(dir + "\\chars\\" + sessionId + ".json");
        }

        public Player GetSessionByName(string name, Player session)
        {
            if (sessions.FirstOrDefault(s => s.Name == name) != null)
            {
                return sessions.FirstOrDefault(s => s.Name == name);
            }

            else return session;
        }

        public void Broadcast(string message, bool info = true)
        {
            message = "<b>" + message + "</b>";
            if (info) message = "⚠ " + message;
            foreach (var session in sessions)
            {
                session.SendMessage(message);
            }
        }

        public void RegenNotInQuest()
        {
            foreach (var session in sessions.FindAll(s=>s.InQuest==false))
            {
                session.Heal(10);
                session.Mana(10);
            }
        }

        public async void StartCombat(Player player1, Player player2)
        {


            player1.InQuest = true;
            player2.InQuest = true;
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
