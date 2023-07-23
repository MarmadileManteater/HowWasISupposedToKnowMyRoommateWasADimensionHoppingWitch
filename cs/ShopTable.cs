using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;


namespace SummerFediverseJam
{

	public class ShopTable : DialogBody
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
			if (player.stats.HasGun)
			{
				return new[]
				{

					new DialogText()
					{
						Name = "Thisbe",
						Text = "Legally, I am not required to sell anything other-"
					},
					new DialogText()
					{
						Name = "Thisbe",
						Text = "than that single fire arm. It is a free country."
					}
				};
			}
			return new[]
			{
					new DialogText {
						Name = "Thisbe",
						Text = "Wanna buy a laser gun?",
						Options = new Dictionary<string, Func<string>>
						{
							{ "Yes", () => { return "ObtainedAGun"; } },
							{ "No", () => { return "WellThatsTooBad";  } }
						}
					},
					new DialogText
					{
						Id = "ObtainedAGun",
						Text = "You obtained a laser gun!",
						OnDisplay = () =>
						{
							player.stats.HasGun = true;
							player.PlayAchievedJingle();
						},
						Next = "LegallyI"
					},
					new DialogText()
					{
						Id = "WellThatsTooBad",
						Name = "Thisbe",
						Text = "Welll, that's too bad."
					},
					new DialogText()
					{
						Id = "LegallyI",
						Name = "Thisbe",
						Text = "Legally, I am not required to sell anything other-"
					},
					new DialogText()
					{
						Name = "Thisbe",
						Text = "than that single fire arm. It is a free country."
					}
			};
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}

}
