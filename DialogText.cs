using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
    public struct DialogText
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Func<string>> Options { get; set; }
        public string Next { get; set; }
        public bool End { get; set; }
        public Action AfterDequeue { get; set; }
    }
}
