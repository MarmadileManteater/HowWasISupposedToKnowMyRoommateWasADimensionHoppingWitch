using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class EntranceIntoParallelDimension : DialogArea
	{
		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap)
			{
				DisableCheck = true;
				if (!__player.stats.IsRoommateInParty)
				{
					__player.NewDialog(new[]
					{
						new DialogText
						{
							Text = "Woah! What is this place?"
						},
						new DialogText
						{
							Text = "It's like my apartment, but also, not."
						}
					});
				} else {
					__player.NewDialog(new[]
{
						new DialogText
						{
							Name = "You",
							Text = "Woah! What is this place?"
						},
						new DialogText
						{
							Name = "Taylor",
							Text = "This is the universe parallel to ours."
						},
						new DialogText
						{
							Name = "Taylor",
							Text = "This is universe 1."
						},
						new DialogText
						{
							Name = "You",
							Text = "What does that make our universe? universe 2?"
						},
						new DialogText
						{
							Name = "Taylor",
							Text = "No!"
						},
						new DialogText
						{
							Name = "Taylor",
							Text = "Our universe is universe A. Look, I started with the alphabet-"
						},
						new DialogText
						{
							Name = "Taylor",
							Text = "but, I realized that I didn't want to invalidate either universe."
						}
					});
				}
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
