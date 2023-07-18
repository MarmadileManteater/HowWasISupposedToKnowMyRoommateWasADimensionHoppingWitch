using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
    internal struct Stats
    {
        public int Health { get; set; }
        public int Sanity { get; set; }
        private int exp;
        public int Exp { 
            get
            {
                return exp;
            }
            set
            {
                if (value > 0)
                    exp += value;
                if (exp > Level * 10)
                {
                    exp = 0;
                    level++;
                }
            }
        }
        private int level;
        public int Level { 
            get
            {
                return level;
            }
        }
        public Guid[] Inventory;
    }
}
