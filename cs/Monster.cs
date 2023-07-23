using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
    public class Monster
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public int ChanceToDie { get; set; }
        public int DamageDone { get; set; }
        public DialogText[] Speak { get; set; }

    }
}
