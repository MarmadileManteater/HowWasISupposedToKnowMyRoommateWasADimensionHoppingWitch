using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
namespace SummerFediverseJam
{
	public class MainGameEndingStart : DialogArea
	{
		private Timer __timer;
		private bool once;
        private AnimatedSprite __roommateDoor;
        private KitchenMask __roommateMask;
        private CollisionShape2D __roommateDoorCollider;
        public override void _Ready()
		{
			__timer = GetNode<Timer>("Timer");
            __roommateDoor = GetParent().GetNode<AnimatedSprite>("Roomate Door");
            __roommateMask = GetParent().GetNode<KitchenMask>("Roomate Room/Roommate mask");
            __roommateDoorCollider = GetParent().GetNode<CollisionShape2D>("Roomate Door Collider/CollisionShape2D");
            base._Ready();
		}
		public void FirstTimeout()
		{
            __roommateMask.ExtraCondition = true;
            __roommateDoor.Play("opendoor");
            __roommateDoor.ZIndex = 2;
            __roommateDoorCollider.Disabled = true;
			__player.GetRoommate().PlayAnimation("WalkIntoBedroom");
            __roommateMask.ForceOn = true;
        }

		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap && __player.stats.IsFunParallelDimensionTimeWRommate && !once)
			{
				__player.NewDialog(new[]
				{
					new DialogText
					{
						Name = "Taylor",
						Text = "There's something I need to show you."
					},
					new DialogText
					{
						Name = "You",
						Text = "Oh?",
						OnDisplay =() =>
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
						Text = "I know that we barely even spend time with each other-"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "-anymore, and I feel like I have neglected you as my friend."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "I would like to apologize for that."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "At the same time, I think this will really cheer you up.",
						AfterDequeue = () =>
						{
							__player.GetRoommate().PlayAnimation($"WalkToBedroom");
							once = true;
							__timer.WaitTime = 5;
							__timer.Connect("timeout", this, nameof(FirstTimeout));
							__timer.Start();
						}
					}
				});
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
