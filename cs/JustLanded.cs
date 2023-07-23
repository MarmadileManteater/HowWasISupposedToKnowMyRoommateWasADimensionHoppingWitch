using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class JustLanded : DialogArea
	{
		private bool once { get; set; }
		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap && !once && __player.stats.IsRoommateInParty)
			{
				once = true;
				DisableCheck = true;
				__player.GetRoommate().Hide();
				__player.NewDialog(new[]
				{
					new DialogText
					{
						Name = "You",
						Text = "Wait, where did you go? Taylor?",
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.Wonder);
						},
						AfterDequeue = () =>
						{
							__player.ShowEmotion(Emotes.None);
						}
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "I'm still with you. Don't be scared."
					},
					new DialogText
					{
						Name = "You",
						Text = "Why shouldn't I be scared? We were messing with dark magic-",
						OnDisplay = () =>
						{
							 __player.ShowEmotion(Emotes.Alert);
						}
					},
					new DialogText
					{
						Name = "You",
						Text = "-and now, I don't know where I am, and I can't see you."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "Calm down. Everything is okay.",
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.None);
						}
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "What you are seeing now is a manifestation of your brain-"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "-if you could see this reality as it is you would go mad."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "You would be unable to function."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "I am currently standing exactly where you are, but-"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "-your brain can't comprehend where I am in space."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "I am going through the exact same thing you are."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "Stop being such a baby!"
					}
				});
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
