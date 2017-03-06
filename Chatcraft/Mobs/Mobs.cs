using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft.Mobs
{
    /// <summary>
    /// Описание мобов
    /// </summary>
    public static class Mobs
    {
        /// <summary>
        /// Список мобов
        /// </summary>
        public static List<Mob> MobList = new List<Mob>()
            {

            new Mob(1, 1, "Гоблин", "маленькое зеленое существо с острыми когтями и хитрым взглядом", 2, 0, 3, new Dictionary<int, int> { { 110, 20 } }, "goblin.png" ),
            new Mob(2, 3, "Тролль", "огромная наглая, грызущая камни, тварь", 5, 2, 10, new Dictionary<int, int> { { 230, 20 }, { 1, 99 } }, "troll.jpg" )

        };
        
        /// <summary>
        /// Вернуть объект моба по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Mob GetMobById(int id)
        {
            return MobList.FirstOrDefault(m => m.Id == id);
        }
        /// <summary>
        /// Вернуть моба по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Mob GetMobByName(string name)
        {
            return MobList.FirstOrDefault(m => m.Name.Equals(name));
        }
        /// <summary>
        /// Вернуть случайного моба заданного уровня (или ниже)
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Mob GetRandomMobByLevel(int level)
        {
            return MobList.FindAll(i => i.Level <= level).OrderBy(i => Guid.NewGuid()).FirstOrDefault();
        }
    }
}
