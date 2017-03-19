using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Chatcraft
{
    public static class MainPage
    {
        public static string id = "MainPage";
        public static string description = "Привет!";
        public static string photo = "stranger.jpg";


        public static ReplyKeyboardMarkup GetKeyboard()
        {
            return Helper.GetKeyboard(new string[] { "Приключения", "Статус", "Магазин" }, new string[] { "Placeholder", "Инвентарь", "Placeholder" });
        }
       
}
}
