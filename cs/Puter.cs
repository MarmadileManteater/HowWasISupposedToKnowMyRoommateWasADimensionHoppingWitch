using Godot;
using System;
using System.Collections.Generic;

namespace SummerFediverseJam
{
	public class Puter : DialogBody
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		private Timer __timer { get; set; }
		private Player __player { get; set; }
		private bool once = false;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			__timer = GetNode<Timer>("Timer");
		}
		public void AfterWork()
		{
			__player.SetSkyColor(new Color("7f73c3"));
			__player.FadeInAptMask();
			__timer.Stop();
			__timer.Disconnect("timeout", this, nameof(AfterWork));
			__player.GetRoommate().Show();
			if (__player.stats.KilledRoommatePlant)
			{
				__player.GetRoommate().PlayAnimation("GettingHomeDeadPlant");
				__player.stats.IsBadRoommateEnding = true;
			} else
			{
				__player.GetRoommate().PlayAnimation("GettingHomePlantIsOkay");
				__player.stats.IsFunParallelDimensionTimeWRommate = true;
			}
		}
		public override DialogText[] GetDialog(Player player)
		{
			if (!once) {
				return new DialogText[]
				{
					new DialogText
					{
						Text = "It is my computer. It is also where I work.",
                        Options = new Dictionary<string, Func<string>>
                        {
                            { "Work", () => { return "Work";  } },
							{ "No", () => { return "NoWork";  } }
                        }
                    },
					new DialogText
					{
						Id = "Work",
						Text = "I guess I should be getting to work. . .",
						OnDisplay = () =>
						{
							once = true;
						},
						AfterDequeue = () =>
						{
							__player = player;
							player.ShowAptMask(0);
							player.FadeOutAptMask();
							__timer.WaitTime = 5;
							__timer.Start();
							__timer.Connect("timeout", this, nameof(AfterWork));
						},
						End = true
					},
                    new DialogText
                    {
                        Id = "NoWork",
                        Text = "I can't work right now. I haven't even woken up yet."
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
