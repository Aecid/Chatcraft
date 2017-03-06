using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Chatcraft.Pages;

namespace Chatcraft
{
    public static class QuestsPage
    {
        public static string id = "Quests";
        public static string pathToTextData = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TextData\\";

        public static InlineKeyboardMarkup GetKeyboard()
        {
            var keyboardList = new List<string>();
            keyboardList.Add("Тёмный Лес ⬆");
            keyboardList.Add("Пещера ⬅");
            keyboardList.Add("Шахта ➡");
            keyboardList.Add("Заброшенный город ↙");
            keyboardList.Add("Назад ⬇");


            return Helper.GetVerticalInlineKeyboardByList(keyboardList);
        }

        public enum questType
        {
            Forest,
            Cave,
            Swamp,
            Praerie,
            Mine,
            LostCity,
            MagicPlace
        }

        public static async void ForestQuest(Session session)
        {
        List<string> StartingMessages = System.IO.File.ReadAllLines(pathToTextData + "ForestStart.txt").ToList();
        List<string> EndingMessages = System.IO.File.ReadAllLines(pathToTextData + "ForestEnd.txt").ToList();


        session.InQuest = true;
            bool encounterTriggered = false;
            bool isQuestCompleted = true;
            Stopwatch s = new Stopwatch();
            s.Start();
            int i = 0;
            int questTime = 5;
            int tickTime = 1;
            int encounterTick = Helper.rnd.Next(1, questTime);

            await session.SendMessage(Helper.GetRandomLine(StartingMessages).Replace("\\n", "\n"));
            while (s.Elapsed < TimeSpan.FromMinutes(questTime))
            {
                i++;
                bool outcome = true;
                await Task.Delay(tickTime*1000*60);
                if (!encounterTriggered && i==encounterTick) {
                outcome = MobEncounter.Start(session, Mobs.Mobs.GetRandomMobByLevel(session.GetLevel()));
                    encounterTriggered = true;
                }
                if (!outcome)
                {
                    isQuestCompleted = false;
                    break;
                }
            }
            
            if (isQuestCompleted) { 
            var reward = new QuestRewards(questType.Forest, session);
            session.AddStatsCounter("Заданий пройдено");
            session.AddStatsCounter("Заданий в Лесу пройдено");
            session.AddGold(reward.gold);
            session.AddExp(reward.exp);
            session.AddItem(reward.items);
            session.InQuest = false;
            session.Persist();
            await session.SendMessage(Helper.GetRandomLine(EndingMessages)+"\n\n" + "Вы успешно прошли квест в Тёмном Лесу!\n" + reward.rewardMessage, MainPage.GetKeyboard());
            }

            s.Stop();
        }

        public static async void CaveQuest(Session session)
        {
         List<string> StartingMessages = System.IO.File.ReadAllLines(pathToTextData + "CaveStart.txt").ToList();
         List<string> EndingMessages = System.IO.File.ReadAllLines(pathToTextData + "CaveEnd.txt").ToList();


        session.InQuest = true;
            bool isQuestCompleted = true;
            Stopwatch s = new Stopwatch();
            s.Start();
            int i = 0;
            int questTime = 10;
            int tickTime = 1;
            int encounterTick = Helper.rnd.Next(1, questTime);
            int encounterTick2 = Helper.rnd.Next(1, questTime);
            while (encounterTick2 == encounterTick)
            {
                encounterTick2 = Helper.rnd.Next(1, questTime - 1);
            }

            await session.SendMessage(Helper.GetRandomLine(StartingMessages));
            while (s.Elapsed < TimeSpan.FromMinutes(questTime))
            {
                i++;
                bool outcome = true;
                await Task.Delay(tickTime*1000*60);
                if (i == encounterTick || i == encounterTick2)
                {
                    outcome = MobEncounter.Start(session, Mobs.Mobs.GetRandomMobByLevel(session.GetLevel()));
                }
                if (!outcome)
                {
                    isQuestCompleted = false;
                    break;
                }
            }

            if (isQuestCompleted)
            {
                var reward = new QuestRewards(questType.Cave, session);
                session.AddStatsCounter("Заданий пройдено");
                session.AddStatsCounter("Заданий в Пещере пройдено");
                session.AddGold(reward.gold);
                session.AddExp(reward.exp);
                session.AddItem(reward.items);
                session.InQuest = false;
                session.Persist();
                await session.SendMessage(Helper.GetRandomLine(EndingMessages) + "\n\n" + "Вы успешно прошли квест в Пещере!\n" + reward.rewardMessage, MainPage.GetKeyboard());
            }

            s.Stop();
        }

        public static async void FailCurrentQuest(Session session)
        {
            if (session.InQuest)
            {
                session.InQuest = false;
                session.AddStatsCounter("Заданий провалено");
                session.AddExp(1);
                await session.SendMessage("Задание провалено. Вы получили 1 опыта за старания");
            }
        }
    }
}
