using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Chatcraft.Common.Pages
{
    public class CastlePage
    {
        /// <summary>
        /// Выбор направления движения
        /// </summary>
        /// <returns></returns>
        public static ReplyKeyboardMarkup GetKeyboardSwitchTurn()
        {
            return Helper.GetKeyboard(new string[] { "Налево", "Направо", "Вниз" });
        }
    }
}
