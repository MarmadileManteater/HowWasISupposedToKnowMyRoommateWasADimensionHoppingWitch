using Godot;
using System;

namespace SummerFediverseJam
{
	public class BadRoommateEndingCard : Node2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		private RichTextLabel __scoreCount { get; set; }
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			__scoreCount = GetNode<RichTextLabel>("ScoreCount");
		}

		public void SetScore(int score)
		{
			__scoreCount.BbcodeText = $"{score}";
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
