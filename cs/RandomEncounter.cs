using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class RandomEncounter : DialogArea
	{
		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap && __player.GetNode<AnimatedSprite>("character").Animation.Contains("walk"))
			{
				var num = new Random().Next(0, 4);
				while (__player.stats.IsOutOfCirculation((MonsterOption)num) && __player.stats.MonstersOutOfCirculation < 4)
				{
					num = new Random().Next(0, 4);
				}
				if (__player.stats.MonstersOutOfCirculation >= 4)
				{
					// No more monsters game end
					__player.NewDialog(new[] {
						new DialogText
						{
							Text = "It doesn't look like there are any more monsters."
						}
					});
					return;
				}
                 __player.EnterBattle((MonsterOption)num);
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
