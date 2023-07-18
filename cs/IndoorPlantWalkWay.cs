using Godot;
using System;
using System.Collections.Generic;

namespace SummerFediverseJam
{
	public class IndoorPlantWalkWay : DialogBody
	{
		private int WaterLevel = 0;
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		public override DialogText[] GetDialog(Player player)
		{
			if (WaterLevel > 3)
			{
				return new[]
				{
					new DialogText {
						Text = "This plant is definitely drowning."
					},
					new DialogText
					{
						Text = "Whoops~ I guess I can move it out of the walk-way now",
						AfterDequeue = () =>
						{
							Hide();
							GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
							var tree = player.GetParent().GetNode<RigidBody2D>("Plant After It Has Drowned");
							tree.Show();
							tree.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
						}
					}
				};
			}
			if (WaterLevel > 2)
			{
				return new[]
				{
					new DialogText
					{
						Text = "The soil is looking very saturated with water.",

					},
					new DialogText {
						Text = "I never liked that it is right in the walk-way.",
						Options = new Dictionary<string, Func<string>> {
							{
								"Water", () => {
									WaterLevel++;
									return "water-too-much";
								}
							},
							{
								"Let be", () =>
								{
									return "let-be";
								}
							}
						}
					},
					new DialogText
					{
						Id = "water-too-much",
						Text = "I think that's definitely too much.",
						End = true,
					},
					new DialogText
					{
						Id = "let-be",
						Text = "I definitely shouldn't over water it.",
						End = true,
					}
				};
			}
			if (WaterLevel > 0)
			{
				return new[]
				{
					new DialogText
					{
						Text = "This plant is looking fairly hydrated.",

					},
					new DialogText {
						Text = "I never liked that it is right in the walk-way.",
						Options = new Dictionary<string, Func<string>> {
							{
								"Water", () => {
									WaterLevel++;
									return "water-more";
								}
							},
							{
								"Let be", () =>
								{
									return "let-be";
								}
							}
						}
					},
					new DialogText
					{
						Id = "water-more",
						Text = "The soil is looking pretty saturated with water.",
						End = true,
					},
					new DialogText
					{
						Id = "let-be",
						Text = "I probably shouldn't over water it.",
						End = true,
					}
				};
			}
			return new[] { 
				new DialogText { 
					Text = "My roomate has always [color=\"red\"]insisted[/color] that this plant stay right here.",
					Options = null
				}, 
				new DialogText {
					Text = "I never liked that it is right in the walk-way.",
					Options = new Dictionary<string, Func<string>> { 
						{ 
							"Water", () => {
								WaterLevel++;
								GD.Print(WaterLevel);
								return "water-fine";
							}
						},
						{
							"Let be", () =>
							{
								return "let-be";
							}
						}
					}
				},
				new DialogText
				{
					Id = "water-fine",
					Text = "The plant seems happy to get some water.",
					End = true
				},
				new DialogText
				{
					Id = "let-be",
					Text = "It will probably be fine.",
					End = true
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
