using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chatcraft.QuestsPage;

namespace Chatcraft
{
    public class QuestRewards
    {
        public long gold { get; set; }
        public long exp { get; set; }
        public List<int> items { get; set; }
        public string rewardMessage;

        public QuestRewards(QuestType quest, Player session)
        {
            Random rnd = new Random();
            items = new List<int>();

            switch (quest)
            {
                case QuestType.Forest:
                    gold = rnd.Next(1, 5);
                    exp = session.Level + rnd.Next(session.Level, session.Level*2+1);
                    if (rnd.Next(1, 101) <= 10)
                    {
                        var r = Items.ItemsList.FindAll(i => i.LvlReq <= session.Level && i.CanBeLooted).OrderBy(i => Guid.NewGuid()).FirstOrDefault();
                        items.Add(r.Id);
                    }
                    break;
                case QuestType.Cave:
                    gold = rnd.Next(5, 15);
                    exp = session.Level + rnd.Next(session.Level*2, session.Level * 3 + 1);
                    if (rnd.Next(1, 101) <= 10)
                    {
                        var r = Items.ItemsList.FindAll(i => i.LvlReq <= session.Level && i.CanBeLooted == true).OrderBy(i => Guid.NewGuid()).FirstOrDefault();
                        items.Add(r.Id);
                    }
                    break;
                case QuestType.Castle:
                    gold = rnd.Next(5, 25);
                    exp = session.Level + rnd.Next(session.Level * 2, session.Level * 3 + 1);
                    if (rnd.Next(1, 101) <= 10)
                    {
                        var r = Items.ItemsList.FindAll(i => i.LvlReq <= session.Level && i.CanBeLooted == true).OrderBy(i => Guid.NewGuid()).FirstOrDefault();
                        items.Add(r.Id);
                    }
                    break;
                default:
                    gold = 0;
                    exp = 0;
                    break;
            }

            var goldMessage = gold == 0 ? "" : "Золото: 💰" + gold.ToString() + "\n";
            var expMessage = exp == 0 ? "" : "Опыт: 🔥" + exp.ToString();
            var itemsMessage = new StringBuilder();
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    itemsMessage.Append($"\n{Items.GetItemName(item)}");
                }
            }
            rewardMessage = "Вы получили: \n" + goldMessage + expMessage + itemsMessage;
        }
    }
}
