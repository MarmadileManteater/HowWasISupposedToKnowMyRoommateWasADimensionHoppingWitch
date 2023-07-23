using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class ShopDoor : DialogArea
	{
		private Timer __timer { get; set; }
		private AudioStreamPlayer __audioStream { get; set; }
		private bool once { get; set; }
		private Action currentAction { get; set; }
		public void FinishAction()
		{
			if (currentAction != null)
			{
				currentAction();
				currentAction = null;
			}
			__timer.Stop();
			__timer.Disconnect("timeout", this, nameof(FinishAction));
		}
		public void DelayAction(Action action, int timeout)
		{
			currentAction = action;
			__timer.WaitTime = timeout;
			__timer.Start();
			__timer.Connect("timeout", this, nameof(FinishAction));
		}
		public override void _Ready()
		{
			__timer = GetNode<Timer>("Timer");
			__audioStream = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
			base._Ready();
		}
		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap && !once && __player.stats.IsRoommateInParty)
			{
				once = true;
				GetParent<Node2D>().Visible = false;
				__audioStream.Play();

				DelayAction(() =>
				{
					GetParent<Node2D>().Visible = true;
					__player.GetParent().RemoveChild(__player);
					__player.CollisionLayer = 1;
					__player.CollisionMask = 1;
					var shop = __player.root.GetNode<Node2D>("Shop");
					shop.Show();
					var entrance = shop.GetNode<Node2D>("Entrance");
					shop.AddChild(__player);
					__player.Position = entrance.Position;
					__player.FaceDirection("u");
					once = false;
				}, 2);
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
