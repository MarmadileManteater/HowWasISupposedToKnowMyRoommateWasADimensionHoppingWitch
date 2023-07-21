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
				__player.NewDialog(new[]
				{
					new DialogText
					{
						Text = "Woah! What is this place?"
					},
					new DialogText
					{
						Text = "It's like my apartment, but all diferenty."
					}
				});
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
