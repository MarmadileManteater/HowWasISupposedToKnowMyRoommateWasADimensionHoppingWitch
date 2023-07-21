using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class PortalToDimensionA : DialogArea
	{
		private AnimationPlayer __animationPlayer;
		private AudioStreamPlayer __audioStreamPlayer;
		private Node2D __perpendicularDimension;
		private Timer __timer;
		private Action currentAction;
		public bool once;
		public override void _Ready()
		{
			__timer = GetNode<Timer>("Timer");
			__timer.WaitTime = 1;
			__perpendicularDimension = GetParent().GetParent<Node2D>();
			__animationPlayer = GetParent().GetNode<AnimationPlayer>("AnimatedSprite/AnimationPlayer");
			__audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
			__animationPlayer.CurrentAnimation = "Bob";
			__animationPlayer.Play();
			base._Ready();
		}
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
		public override void NotifyOverlapChange(bool overlap)
		{
			if (currentAction == null && overlap)
			{
				__player.PauseBackgroundMusic();
				__audioStreamPlayer.Play();
				__player.CollapseDimension();

				
				DelayAction(() =>
				{
					__animationPlayer.Stop();
					__perpendicularDimension.Hide();
					__player.GetParent().RemoveChild(__player);
					__player.CollisionLayer = 1;
					__player.CollisionMask = 1;
					var node = __player.root.GetNode<TileMap>("Environment layer 2");
					__player.root.AddChildBelowNode(__player.root.GetNode<TileMap>("Environment layer 2"), __player);
					__player.Position = new Vector2(491.816f, -273.374f);
					__player.FaceDirection("d");
					__player.HideAptMask();
					__player.ExpandDimension();
					__perpendicularDimension.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
					__player.PlayBackgroundMusic();
				}, 2);
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
