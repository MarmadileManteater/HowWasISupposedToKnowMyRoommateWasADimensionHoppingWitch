using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
namespace SummerFediverseJam
{
	public class BadRoommateEnding : DialogArea
	{
		private Timer __timer;
		private bool hasEndeded;
		public override void _Ready()
		{
			__timer = GetNode<Timer>("Timer");
			base._Ready();
		}

		public void GameEnd()
		{
			if (!hasEndeded)
			{
				__player.FadeOutAptMask();
				__timer.WaitTime = 2;
				hasEndeded = true;
				__timer.Start();
			} else {
				__player.ShowBadEndCard();
			}
		}
		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap && __player.stats.IsBadRoommateEnding)
			{
				__player.NewDialog(new[]
				{
					new DialogText
					{
						Name = "Taylor",
						Text = "You murdered my plant!"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "You are a plant murderer!"
					},
					new DialogText
					{
						Name = "You",// how did you come to that conclusion, the "you" conclusion?
						Text = "I'm sorry. I just wanted your attention."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "Is this about how you didn't like how it was in the walk-way?"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "You are so immature!",
						OnDisplay = () =>
						{
							__player.GetRoommate().ShowEmotion(Emotes.Alert);
						},
						AfterDequeue = () =>
						{
							__player.GetRoommate().ShowEmotion(Emotes.None);
							__player.GetRoommate().PlayAnimation("StormingOffBadEnding");
							__timer.WaitTime = 5;
							__timer.OneShot = true;
							__timer.Start();
							__timer.Connect("timeout", this, nameof(GameEnd));
							__player.GameOver = true;
						}
					}
				});
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
