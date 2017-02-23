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
        private List<Session> sessions;
        string dir;

        public SessionStorage()
        {
            sessions = new List<Session>();
            dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public Session GetSession(long chatId, string username = "")
        {
            if (sessions.Exists(s => s.id == chatId))
            {
                return sessions.FirstOrDefault(s => s.id == chatId);
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
                    var currentSession = new Session();
                    currentSession.id = chatId;
                    currentSession.username = username.Equals(string.Empty)?"UnnamedPlayer":username;
                    currentSession.name = username;
                    currentSession.pageId = "MainPage";
                    sessions.Add(currentSession);
                    return currentSession;
                }
            }
        }

        public List<Session> GetSessions()
        {
            return sessions;
        }

        public void SerializeSession(Session session)
        {
            Helper.WriteToJsonFile(dir + "\\chars\\" + session.id + ".json", session);
        }

        public Session DeserializeSession(string sessionId)
        {
            return Helper.ReadFromJsonFile<Session>(dir + "\\chars\\" + sessionId + ".json");
        }

        public Session GetSessionByName(string name, Session session)
        {
            if (sessions.FirstOrDefault(s => s.name == name) != null)
            {
                return sessions.FirstOrDefault(s => s.name == name);
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
            foreach (var session in sessions.FindAll(s=>s.inQuest==false))
            {
                session.Heal(10);
                session.Mana(10);
            }
        }

        public async void StartCombat(Session player1, Session player2)
        {


            player1.inQuest = true;
            player2.inQuest = true;
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
