using System;
using System.Collections.Generic;
using System.Text;

namespace Chatcraft.Common
{
    /// <summary>
    /// Класс с глобальными переменными (для всех игроков)
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Максимальное время находждения игрока в квесте
        /// Нужно для проверки того, что игрок не завис в нем
        /// </summary>
        public static int MAX_QUEST_TIME_MIN = 20;
    }
}
