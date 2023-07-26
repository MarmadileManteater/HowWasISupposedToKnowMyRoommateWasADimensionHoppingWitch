using Godot;
using System;

namespace SummerFediverseJam
{
	public enum StartMenuOption
	{
		Reset,
		ReturnToGame
	}
	public class StartMenu : CanvasLayer
	{
		private AnimatedSprite selector;
		private RichTextLabel reset;
		private RichTextLabel returnToGame;
		private StartMenuOption selected;
		[Export]
		public StartMenuOption Selected
		{
			get
			{
				return selected;
			}
			set
			{
				selected = value;
				selector.Position = new Vector2(selector.Position.x, 250 + (int)value * 90);
			}
		}
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			reset = GetNode<RichTextLabel>("Node2D/ColorRect/Reset");
			returnToGame = GetNode<RichTextLabel>("Node2D/ColorRect/Return to Game");
			selector = GetNode<AnimatedSprite>("Node2D/Selector");
			selected = StartMenuOption.Reset;
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (GetTree().Paused)
			{
				int option = (int)Selected;
				if (@event.IsActionPressed("ui_up"))
				{
					option--;
				}
				if (@event.IsActionPressed("ui_down"))
				{
					option++;
				}
				if (option > 1)
				{
					option = 0;
				}
				if (option < 0)
				{
					option = 1;
				}
				Selected = (StartMenuOption)option;
				if (@event.IsActionPressed("ui_accept"))
				{
					ExecuteChoice();
				}
			}
			base._UnhandledInput(@event);
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }

		private void ExecuteChoice()
		{

			switch (Selected)
			{
				case StartMenuOption.Reset:
					GetTree().Paused = false;
					GetTree().ReloadCurrentScene();
					break;
				case StartMenuOption.ReturnToGame:
					Hide();
					GetTree().Paused = false;
					break;
			}
		}

		private void _on_Return_to_Game_mouse_entered()
		{
			// Replace with function body.
			Selected = StartMenuOption.ReturnToGame;
		}
		
		private void _on_Reset_mouse_entered()
		{
			// Replace with function body.
			Selected = StartMenuOption.Reset;
		}


		private void _on_return_input(object @event)
		{
			GD.Print(@event);
			Selected = StartMenuOption.Reset;
			_on_gui_input(@event);
		}

		private void _on_reset_input(object @event)
		{
			Selected = StartMenuOption.ReturnToGame;
			_on_gui_input(@event);
		}

		private void _on_gui_input(object @event)
		{
			if (@event is InputEventMouseButton mouseEvent)
			{
				if (mouseEvent.IsPressed())
				{
					if (mouseEvent.ButtonIndex == 1)
					{
						if (GetTree().Paused)
						{
							ExecuteChoice();
						}
					}
				}
			}
		}

	}

}
