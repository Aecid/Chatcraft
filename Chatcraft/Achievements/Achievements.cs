using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    public static class Achievements
    {
        public static List<Achievement> AchList = new List<Achievement>()
        {
            new Achievement(1, "Заданий пройдено", 100, "Приключенец", "Пройдено 100 квестов!"),
            new Achievement(2, "Убито врагов \"Гоблин\"", 100, "Убийца гоблинов", "Убито 100 гоблинов!"),
            new Achievement(3, "Убито врагов \"Тролль\"", 100, "Уничтожитель троллей", "Убито 100 троллей!"),
            new Achievement(4, "Заданий в Лесу пройдено", 100, "Рейнджер", "Пройдено 100 квестов в Лесу"),
            new Achievement(5, "Заданий в Пещере пройдено", 100, "Спелеолог", "Пройдено 100 квестов в Пещере"),
            new Achievement(99, "Получено первое достижение в игре", 1, "Мастер ачивок", "Пройдено 100 квестов в Пещере"),
        }
        ;
        
        public static Achievement GetAchByStatName(string stat)
        {
            return AchList.FirstOrDefault(a => a.stat.Equals(stat));
        }

        public static Achievement GetAchById(int id)
        {
            return AchList.FirstOrDefault(a => a.id == id);
        }
    }
}
