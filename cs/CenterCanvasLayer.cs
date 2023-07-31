using Godot;
using System;

public class CenterCanvasLayer : CanvasLayer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Transform = new Transform2D(Transform.x, Transform.y, new Vector2((GetViewport().GetVisibleRect().Size.x / 2) - 500, Transform.origin.y));
	}
}
