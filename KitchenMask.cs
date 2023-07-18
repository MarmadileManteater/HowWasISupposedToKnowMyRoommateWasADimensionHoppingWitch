using Godot;
using SummerFediverseJam;
using System;

public class KitchenMask : ColorRect
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private AnimationPlayer __animationPlayer;
	private Player __player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		__animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		__player = GetParent().GetNode<Player>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (__player.Position.x < RectPosition.x + RectSize.x && __player.Position.x > RectPosition.x &&
			__player.Position.y < RectPosition.y + RectSize.y + 24 * 3 && __player.Position.y > RectPosition.y) {
			if (Color.a == 1)
			{
				__animationPlayer.CurrentAnimation = "Fade in";
			}
		} else if (Color.a == 0) {
			__animationPlayer.CurrentAnimation = "Fade out";
		}
	}
}
