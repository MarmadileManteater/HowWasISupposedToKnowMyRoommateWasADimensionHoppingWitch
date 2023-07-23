using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class PortalToDimension1 : DialogArea
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
			if (currentAction == null && overlap && !__player.IsTransitionCurrentlyHappening)
			{
				__player.IsTransitionCurrentlyHappening = true;

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
					var parallelDimension = __player.root.GetNode<Node2D>("Parallel Dimension");
					parallelDimension.Show();
					var entrance = parallelDimension.GetNode<Node2D>("Entrance");
					parallelDimension.AddChildBelowNode(parallelDimension.GetNode<Node2D>("Portal to Dimension 1"), __player);
					__player.Position = entrance.Position;
					__player.FaceDirection("d");
					__player.ExpandDimension();
					__perpendicularDimension.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
					parallelDimension.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
					__player.IsTransitionCurrentlyHappening = false;

				}, 2);
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
