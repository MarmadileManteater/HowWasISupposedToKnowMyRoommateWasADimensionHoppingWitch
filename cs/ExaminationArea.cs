using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class ExaminationArea : DialogArea
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
						Text = "Is this even real?",
						TextSpeed = 0.100f,
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.Wonder);
						}
					},
					new DialogText
					{
						Text = "And, if it is? Is this really right?"
					},
					new DialogText
					{
						Text = "I intentionally killed my roommates plant, and"
					},
					new DialogText
					{
						Text = "I am now snooping around in their room. Should I keep going?",
						AfterDequeue = () =>
						{
							__player.ShowEmotion(Emotes.None);
						}
					}
				});
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
