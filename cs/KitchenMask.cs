using Godot;
using SummerFediverseJam;
using System;

public class KitchenMask : ColorRect
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public bool ExtraCondition = true;
	[Export]
	public bool ForceOn = false;
	[Export]
	public int BottomPadding = 24 * 3;

	private AnimationPlayer __animationPlayer;
	private Player __player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		__animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		var parent = GetParent();
		while (__player == null) {
			if (parent.HasNode("Player"))
			{
				__player = parent.GetNode<Player>("Player");
			}
			parent = parent.GetParent();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (ExtraCondition)
		{
			var modifier = 1;
			if (__player.GetParent<Node2D>().Scale.x < 0)
			{
				modifier = -1;
			}
            var position = RectGlobalPosition * modifier;
			if (ForceOn || __player.Position.x < position.x + RectSize.x && __player.Position.x > position.x &&
				__player.Position.y < position.y + RectSize.y + BottomPadding * (modifier == -1?2:1) && __player.Position.y > position.y)
			{
				if (Color.a == 1)
				{
					__animationPlayer.CurrentAnimation = "Fade in";
				}
			}
			else if (Color.a == 0)
			{
				__animationPlayer.CurrentAnimation = "Fade out";
			}
		}
	}
}
