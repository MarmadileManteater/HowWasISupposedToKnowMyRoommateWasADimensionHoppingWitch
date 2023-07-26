using Godot;
using SummerFediverseJam;
using System;
using System.Linq;

public class Dialog : Node2D
{
	public int PhraseNum = -1;
	private Timer __timer { get; set; }
	private RichTextLabel __name { get; set; }
	private RichTextLabel __text { get; set; }
	private AnimatedSprite __indicator { get; set; }
	private ColorRect __decideTimeBox { get; set; }
	private RichTextLabel[] __options { get; set; }
	private AnimatedSprite __selector { get; set; }
	[Export]
	private float TextSpeed = 0.025f;
	public DialogText[] dialog { get; set; }
	private int SelectedOption { get; set; }
	private bool HoveringA { get; set; }
	private bool HoveringB { get; set; }
	public bool TouchEnabled { get; set; }
	// this is a mutilpier:
	// - x1 if the player is not mashing enter
	// - x2 of the player is
	private int TextSpeedModifier { get; set; }
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		__timer = GetNode<Timer>("Timer");
		__name = GetNode<RichTextLabel>("Box/Name");
		__text = GetNode<RichTextLabel>("Box/Text");
		__indicator = GetNode<AnimatedSprite>("Indicator");
		__decideTimeBox = GetNode<ColorRect>("DecideTimeBox");
		__options = new[] { GetNode<RichTextLabel>("DecideTimeBox/Option A"), GetNode<RichTextLabel>("DecideTimeBox/Option B") };
		__selector = GetNode<AnimatedSprite>("DecideTimeBox/Selector");
		__timer.WaitTime = TextSpeed;

	}

	private int FindDialogChoiceById(string id)
	{
		for (int i = 0; i < dialog.Length; i++)
		{
			var d = dialog[i];
			if (d.Id != null)
			{
				if (d.Id.Equals(id))
				{
					return i;
				}
			}
		}
		return -1;
	}

	public void TypewriterLoop()
	{
		if (__text.VisibleCharacters < __text.Text.Length)
		{
			__text.VisibleCharacters++;
			__indicator.Hide();
		} else {
			__timer.Stop();
			__indicator.Show();
		}
	}

	public void NextPhrase(bool IsMouseInput = false)
	{
		string choice = null;
		if (IsMouseInput && PhraseNum > -1 && dialog[PhraseNum].Options != null && dialog[PhraseNum].Options.Count != 0 && !(HoveringA || HoveringB || TouchEnabled))
		{
			return;
		}
		if (__text.VisibleCharacters < __text.Text.Length)
		{
			// treat the player mashing a as increasing the reveal speed of the text
			__text.VisibleCharacters++;
			return;
		}
		__indicator.Hide();
		if (PhraseNum != -1) {
			if (dialog[PhraseNum].AfterDequeue != null)
			{
				dialog[PhraseNum].AfterDequeue();
			}
			if (dialog[PhraseNum].End)
			{
				Close();
				return;
			}
			if (dialog[PhraseNum].Options != null)
			{
				var selectedOption = dialog[PhraseNum].Options.ToList()[SelectedOption];
				var id = selectedOption.Value.Invoke();
				PhraseNum = FindDialogChoiceById(id);
			} else if (dialog[PhraseNum].Next != null) {
				PhraseNum = FindDialogChoiceById(dialog[PhraseNum].Next);
			} else {
				PhraseNum++;
			}
		} else {
			PhraseNum++;
		}
		if (choice == null) {
			if (PhraseNum < dialog.Length) {
				if (dialog[PhraseNum].Name == null)
				{
					GetNode<ColorRect>("Name").Hide();

				}
				else
				{
					GetNode<ColorRect>("Name").Show();
				}
				__name.BbcodeText = dialog[PhraseNum].Name;
				__text.BbcodeText = dialog[PhraseNum].Text;
				if (dialog[PhraseNum].OnDisplay != null)
				{
					dialog[PhraseNum].OnDisplay();
				}
				__text.VisibleCharacters = 0;
				if (dialog[PhraseNum].TextSpeed != 0)
				{
					__timer.WaitTime = dialog[PhraseNum].TextSpeed;
				} else {
					__timer.WaitTime = TextSpeed;

				}
				if (dialog[PhraseNum].Options != null)
					{
					if (dialog[PhraseNum].Options.Count > __options.Length)
					{
						GD.PushWarning("Options for dialog text are greater than currently supported number");
					}
					for (int i = 0; i < __options.Length; i++)
					{
						__options[i].Text = dialog[PhraseNum].Options.ToList()[i].Key;
					}
				}
				if (!__timer.IsConnected("timeout", this, nameof(TypewriterLoop)))
				{
					__timer.Connect("timeout", this, nameof(TypewriterLoop));
				}
				__timer.Start();
			} else {
				Close();
			}
		} else {
			PhraseNum = FindDialogChoiceById(dialog[PhraseNum].Next);
		}
	}

	private void Close()
	{
		PhraseNum = -1;
		dialog = new DialogText[0];
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (__decideTimeBox.Visible)
		{
			if (@event.IsActionPressed("ui_up"))
			{
				SelectedOption -= 1;
			}
			if (@event.IsActionPressed("ui_down"))
			{
				SelectedOption += 1;
			}
			if (SelectedOption < 0)
			{
				SelectedOption = 1;
			}
			if (SelectedOption > 1)
			{
				SelectedOption = 0;
			}
		}
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (GetParent().GetParent() is Player player)
		{
			TouchEnabled = player.TouchEnabled;
		}
		if (PhraseNum < 0) {
			Hide();
		} else {
			if (PhraseNum  >= 0 && dialog.Length > PhraseNum && dialog[PhraseNum].Options != null && __text.VisibleCharacters >= __text.Text.Length)
			{
				__decideTimeBox.Show();
				__selector.Position = new Vector2(0,7 + SelectedOption * 10);
				
			} else
			{
				__decideTimeBox.Hide();
			}
			Show();
		}
	}

	private void _on_Option_A_mouse_entered()
	{
		GetNode<Control>("DecideTimeBox/Option A").MouseDefaultCursorShape = Control.CursorShape.PointingHand;
		SelectedOption = 0;
		HoveringA = true;
	}


	private void _on_Option_A_mouse_exited()
	{
		HoveringA = false;
	}


	private void _on_Option_B_mouse_entered()
	{
		GetNode<Control>("DecideTimeBox/Option B").MouseDefaultCursorShape = Control.CursorShape.PointingHand;
		SelectedOption = 1;
		HoveringB = true;
	}


	private void _on_Option_B_mouse_exited()
	{
		HoveringB = false;
	}

}

