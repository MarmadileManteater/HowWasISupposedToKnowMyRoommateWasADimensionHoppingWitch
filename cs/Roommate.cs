using Godot;
using System;

namespace SummerFediverseJam
{
	public class Roommate : KinematicBody2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		private AnimatedSprite __emotes;
		private AnimatedSprite __sprite;
		private AnimationPlayer __animationPlayer;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			__emotes = GetNode<AnimatedSprite>("Emotes");
			__sprite = GetNode<AnimatedSprite>("roommate");
			__animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		}

		public void ShowEmotion(Emotes emote)
		{
			switch (emote)
			{
				case Emotes.Alert:
					__emotes.Animation = "alert";
					break;
				case Emotes.Wonder:
					__emotes.Animation = "wonder";
					break;
				case Emotes.Love:
					__emotes.Animation = "love";
					break;
				case Emotes.None:
					__emotes.Animation = "default";
					break;
			}
		}

		public void FaceDirection(string direction)
		{
			__sprite.Animation = $"{direction}default";
		}

		public void PlayAnimation(string name)
		{
			__animationPlayer.CurrentAnimation = name;
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
