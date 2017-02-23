using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    public static class Shop
    {
        public static void BuyItems(Session session)
        {
            string itemList = "";
            foreach (var item in Items.ItemsList.FindAll(i => i.canBeBought == true))
            {
                itemList += "\n<b>" + item.name + "</b> " + " (💰" + item.price + ")" + "\n" + Items.GetShortItemInfo(item.id) + "\n/buy_"+item.id + "\n";
            }

            session.SendMessage(itemList);
        }

        public static void BuyItems(Session session, string slot)
        {
            string itemList = "";
            foreach (var item in Items.ItemsList.FindAll(i => i.slot == slot && i.canBeBought == true))
            {
                itemList += "\n<b>" + item.name + "</b> " + " (💰" + item.price + ")" + "\n" + Items.GetShortItemInfo(item.id) + "\n/buy_" + item.id + "\n";
            }

            session.SendMessage(itemList);
        }

        public static void SellItems(Session session)
        {
            string itemList = "";
            foreach (var itemId in session.items)
            {
                var item = Items.GetItemById(itemId);
                itemList += "\n" + item.name + " " + item.price/3 + "💰" + " /sell_" + item.id;
            }

            session.SendMessage(itemList);
        }
    }
}
