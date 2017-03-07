using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    /// <summary>
    /// Достижения
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Статистика
        /// </summary>
        public string Stat { get; private set; }
        /// <summary>
        /// Количество
        /// </summary>
        public long Count { get; private set; }
        /// <summary>
        /// Наименование достижения
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Описание достижения
        /// </summary>
        public string Description { get; private set; }

        public Achievement(int id, string stat, long count, string name, string desc)
        {
            Id = id;
            Stat = stat;
            Count = count;
            Name = name;
            Description = desc;
        }
    }
}
