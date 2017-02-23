using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft
{
    public class Achievement
    {
        public int id;
        public string stat;
        public long count;
        public string name;
        public string description;

        public Achievement(int _id, string _stat, long _count, string _name, string _desc)
        {
            id = _id;
            stat = _stat;
            count = _count;
            name = _name;
            description = _desc;
        }
    }
}
