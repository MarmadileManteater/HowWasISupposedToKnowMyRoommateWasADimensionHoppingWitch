using Godot;
using System;
using System.Linq;

namespace SummerFediverseJam
{

    public class MundaneThing : DialogBody
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }

        protected DialogText[] AddDialog(DialogText[] dialogA, DialogText[] dialogB)
        {
            var dialogs = dialogA.ToList();
            dialogs.AddRange(dialogB);
            return dialogs.ToArray();
        }

        protected DialogText[] AddBaseDialog(Player player, DialogText[] given)
        {
            var d = AddDialog(GetMundaneDialog(player), given);
            var action = d[d.Length - 1].AfterDequeue;
            d[d.Length - 1].AfterDequeue = () =>
            {
                player.InteractWithObject(this);
                if (action != null)
                    action();
            };
            return d;
        }

        public DialogText[] GetMundaneDialog(Player player)
        {
            if (player.MundaneObjectsCount == 3)
            {
                player.stats.LooksAtMundaneObjects = true;
                return new[]
                {
                    new DialogText
                    {
                        Text = "Wow, I am just facinated with every little object today."
                    }
                };
            }
            return new DialogText[0];
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }

}
