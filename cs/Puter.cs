using Godot;
using System;

namespace SummerFediverseJam
{
	public class Puter : DialogBody
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		private Timer __timer { get; set; }
		private Player __player { get; set; } 
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			__timer = GetNode<Timer>("Timer");
		}
		public void AfterWork()
		{
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
			return new DialogText[]
			{
				new DialogText
				{
					Text = "Well, I guess I should be getting to work. . .",
					AfterDequeue = () =>
					{
						__player = player;
						player.FadeOutAptMask();
						__timer.WaitTime = 5;
						__timer.Start();
						__timer.Connect("timeout", this, nameof(AfterWork));
					}
				}
			};
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
