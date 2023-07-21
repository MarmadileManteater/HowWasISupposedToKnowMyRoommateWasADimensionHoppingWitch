using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class AmazementArea : DialogArea
	{
		private bool once { get; set; }
		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap && !once && __player.stats.KilledRoommatePlant && __player.stats.UsedRoommatesPlantToOpenDoor)
			{
				once = true;
				__player.NewDialog(new[]
				{
					new DialogText
					{
						Text = "What . is . that .",
						TextSpeed = 0.100f,
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.Alert);
						}
					},
					new DialogText
					{
						Text = "*has my roommate secretly forged a portal ",
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.Wonder);
						}
					},
					new DialogText
					{
						Text = "by invoking powerful dark magic behind my back???*",
						TextSpeed = 0.050f
					},
					new DialogText
					{
						Text = "Have they even considered how much trouble we would get in"
					},
					new DialogText
					{
						Text = "if our landlord finds out we didn't pay the deposit"
					},
					new DialogText
					{
						Text = "specifically for black magic use?"
					},
					new DialogText
					{
						Text = "I mean.",
						TextSpeed = 0.050f,
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.None);

						}
					},
					new DialogText
					{
						Text = "It's like 100 dollars a month per practioner."
					},
					new DialogText
					{
						Text = "That's coming out of the deposit."
					}
				});
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
