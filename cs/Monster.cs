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
        public Dictionary<string, Action<Battle>> SpeakOptions { get; set; }
        public Action<Battle>[] Actions { get; set; }
    }
}
