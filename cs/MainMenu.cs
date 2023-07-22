using Godot;
using System;
namespace SummerFediverseJam
{
	public class MainMenu : Node2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		private AnimationPlayer __startCommandMask;
		private AudioStreamPlayer __stream;
		private Timer __timer;
		public bool isPlaying;
		public bool canStart;

		public void AlarmTimeout()
		{

			if (!isPlaying) {
				__startCommandMask.CurrentAnimation = "Fade in";
				__timer.Stop();
				__stream.Play();
				canStart = true;
			}
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			__startCommandMask = GetNode<AnimationPlayer>("ColorRect/AnimationPlayer");
			__stream = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
			__timer = GetNode<Timer>("Timer");
			__timer.WaitTime = 2;
			__timer.Connect("timeout", this, nameof(AlarmTimeout));
			__timer.Start();
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed("ui_accept") && !isPlaying && canStart)
			{
				__stream.Stop();
				__startCommandMask.CurrentAnimation = null;
				Hide();
				GetParent().GetNode<Player>("Player").HideAptMask();
				GetParent().GetNode<Player>("Player").PlayBackgroundMusic();
				isPlaying = true;

			}
			base._UnhandledInput(@event);
		}
	}

}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
