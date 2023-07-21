using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class PortalToPerpendicularUniverse : DialogArea
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
			__perpendicularDimension = GetParent().GetParent().GetNode<Node2D>("Perpendicular Dimension");
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
					__perpendicularDimension.Show();
					__player.GetParent().RemoveChild(__player);
					__player.CollisionLayer = 4;
					__player.CollisionMask = 4;
					__perpendicularDimension.AddChildBelowNode(__perpendicularDimension.GetNode<Node2D>("Portal to Dimension A"), __player);
					__player.Position = new Vector2(514.414f, 180.354f);
					__player.FaceDirection("d");
					__player.ShowAptMask();
					__player.ExpandDimension();
					__perpendicularDimension.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
				}, 2);
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
