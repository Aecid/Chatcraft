using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    public static class Shop
    {
        public static void BuyItems(Player session)
        {
            string itemList = "";
            foreach (var item in Items.ItemsList.FindAll(i => i.CanBeBought == true))
            {
                itemList += "\n<b>" + item.Name + "</b> " + " (💰" + item.Price + ")" + "\n" + Items.GetShortItemInfo(item.Id) + "\n/buy_"+item.Id + "\n";
            }

            session.SendMessage(itemList);
        }

        public static void BuyItems(Player session, string slot)
        {
            string itemList = "";
            foreach (var item in Items.ItemsList.FindAll(i => i.Slot == slot && i.CanBeBought == true))
            {
                itemList += "\n<b>" + item.Name + "</b> " + " (💰" + item.Price + ")" + "\n" + Items.GetShortItemInfo(item.Id) + "\n/buy_" + item.Id + "\n";
            }

            session.SendMessage(itemList);
        }

        public static void SellItems(Player session)
        {
            string itemList = "";
            foreach (var itemId in session.Items)
            {
                var item = Items.GetItemById(itemId);
                itemList += "\n" + item.Name + " " + item.Price/3 + "💰" + " /sell_" + item.Id;
            }

            session.SendMessage(itemList);
        }
    }
}
