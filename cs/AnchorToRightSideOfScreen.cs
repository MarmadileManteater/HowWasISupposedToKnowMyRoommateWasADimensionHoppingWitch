using Godot;
using System;

public enum Side
{
	Right,
	Center
}
public class AnchorToRightSideOfScreen : Node2D
{
	[Export]
	public Side Side { get; set; }
	[Export]
	public int offset = 600;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	private void SetPositionToOffsetOfRightSideOfScreen(float offset)
	{
		Vector2 screenRect = GetViewportRect().Size;
		if (Side == Side.Right) {
			Position = new Vector2(screenRect.x - offset, Position.y);
		} else if (Side == Side.Center) {
			Position = new Vector2(screenRect.x / 2 - offset, Position.y);
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		SetPositionToOffsetOfRightSideOfScreen(offset);
	}
}
