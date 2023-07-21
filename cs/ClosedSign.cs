using Godot;
using System;

namespace SummerFediverseJam
{
	public class ClosedSign : DialogBody
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			base._Ready();
		}

		public override DialogText[] GetDialog(Player player)
		{
			if (player.stats.UsedRoommatesPlantToOpenDoor)
			{
				return new[]
				{
						new DialogText
						{
							Text = "There is a sign on the door."
						},
						new DialogText {
							Text = "\"The Perpendicular Shop is closed at the moment."
						},
						new DialogText {
							Text = "It will be open again sometime between now and never.\""
						}
				};
			}

			return new DialogText[0];
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
