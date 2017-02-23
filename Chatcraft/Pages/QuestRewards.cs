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

        public QuestRewards(questType quest, Session session)
        {
            Random rnd = new Random();
            items = new List<int>();

            switch (quest)
            {
                case questType.Forest:
                    gold = rnd.Next(1, 5);
                    exp = session.level + rnd.Next(session.level, session.level*2+1);
                    if (rnd.Next(1, 101) <= 10)
                    {

                        var r = Items.ItemsList.FindAll(i => i.lvlReq <= session.level && i.canBeLooted == true).OrderBy(i => Guid.NewGuid()).FirstOrDefault();
                        items.Add(r.id);
                    }
                    break;
                case questType.Cave:
                    gold = rnd.Next(5, 15);
                    exp = session.level + rnd.Next(session.level*2, session.level * 3 + 1);
                    if (rnd.Next(1, 101) <= 10)
                    {

                        var r = Items.ItemsList.FindAll(i => i.lvlReq <= session.level && i.canBeLooted == true).OrderBy(i => Guid.NewGuid()).FirstOrDefault();
                        items.Add(r.id);
                    }
                    break;
                default:
                    gold = 0;
                    exp = 0;
                    break;
            }

            var goldMessage = gold == 0 ? "" : "Золото: 💰" + gold.ToString() + "\n";
            var expMessage = exp == 0 ? "" : "Опыт: 🔥" + exp.ToString();
            string itemsMessage = "";
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    itemsMessage += "\n" + Items.GetItemName(item);
                }
            }
            rewardMessage = "Вы получили: \n" + goldMessage + expMessage + itemsMessage;
        }
    }
}
