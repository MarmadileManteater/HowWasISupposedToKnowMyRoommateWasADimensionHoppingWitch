using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;


namespace SummerFediverseJam
{

	public class Sink : MundaneThing
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}
		
		public override DialogText[] GetDialog(Player player)
		{
            return AddBaseDialog(player, new[]
            {
                    new DialogText {
                        Text = "This is where we wash our hands."
                    }
			});
        }

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
	}

}
