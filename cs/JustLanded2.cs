using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class JustLanded2 : DialogArea
	{
		private bool once { get; set; }
		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap && !once && __player.stats.IsRoommateInParty)
			{
				once = true;
				DisableCheck = true;
				__player.NewDialog(new[]
				{
					new DialogText
					{
						Name = "You",
						Text = "So, I'm just spitballing from what I have seen so far."
					},
					new DialogText
					{
						Name = "You",
						Text = "From the manifestation I see, this seems to be some sort of-"
					},
					new DialogText
					{
						Name = "You",
						Text = "-perpendicular universe?"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "It is a universe perpendiuclar our original universe-"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "-which I conjured.",
						TextSpeed = 0.010f
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "It is sort of a micro bridge universe between us-"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "-and a parallel universe. "
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "To be fair, I'm kind of new at this, and I didn't realize-"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "-that sapient, sentient beings would begin to populate, but-"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "-like if you go up that middle path, you'll find a shop owner."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "He wasn't here when I conjured this place, but he is now."
					}
				});
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
