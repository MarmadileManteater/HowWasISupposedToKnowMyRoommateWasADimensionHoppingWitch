using Godot;

using System;
using System.Collections.Generic;

namespace SummerFediverseJam {
	public class Player : KinematicBody2D
	{
		private Dialog __dialog;
		private Vector2 DialogExpireLocation = new Vector2(0, 0);
		public Controls Controls = new Controls();
		#region Mundane Objects
		private List<MundaneThing> MundaneObjectsInteractedWith = new List<MundaneThing>();
		// mundane objects count
		public int MundaneObjectsCount {
			get {
				return MundaneObjectsInteractedWith.Count;
			}
		}
		public void InteractWithObject(MundaneThing thing)
		{
			if (MundaneObjectsInteractedWith.IndexOf(thing) == -1)
			{
				MundaneObjectsInteractedWith.Add(thing);
			}
		}
		#endregion
		
		public override void _Ready()
		{
			__dialog = GetNode<Dialog>("Dialog");
			__dialog.dialog = new DialogText[0]; 
		}
		
		public void NewDialog(DialogText[] dialog)
		{
			__dialog.dialog = dialog;
			__dialog.NextPhrase();
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed("ui_left")) {
				Controls.Left = true;
			}
			if (@event.IsActionReleased("ui_left"))
			{
				Controls.Left = false;
			}
			if (@event.IsActionPressed("ui_right"))
			{
				Controls.Right = true;
			}
			if (@event.IsActionReleased("ui_right"))
			{
				Controls.Right = false;
			}
			if (@event.IsActionPressed("ui_up"))
			{
				Controls.Up = true;
			}
			if (@event.IsActionReleased("ui_up"))
			{
				Controls.Up = false;
			}
			if (@event.IsActionPressed("ui_down"))
			{
				Controls.Down = true;
			}
			if (@event.IsActionReleased("ui_down"))
			{
				Controls.Down = false;
			}
			if (@event.IsActionPressed("ui_accept"))
			{
				__dialog.NextPhrase();
			}
			base._UnhandledInput(@event);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			
			var sprite = GetNode<AnimatedSprite>("character");
			// only want to set animation once per `_Process`
			// otherwise the animation won't play if it keeps flipping back and forth before the animation plays
			var animation = sprite.Animation;
			// only activate controls if the dialog is closed or unpopulated
			if (__dialog.PhraseNum == -1) {
				Godot.Object collider = null;
				Vector2 direction = Vector2.Zero;
				if (Controls.Left) {
					direction += Vector2.Left;
					animation = "lwalk";
				}
				if (Controls.Right) {
					direction += Vector2.Right;
					animation = "rwalk";
				}
				if (Controls.Up) {
					direction += Vector2.Up;
					animation = "uwalk";
				}
				if (Controls.Down) {
					direction += Vector2.Down;
					animation = "dwalk";
				}
				if (direction != Vector2.Zero)
				{
					MoveAndSlide(direction * delta * 10000, Vector2.Up, false, 4, (float) Math.PI / 4, false);
					collider = GetLastSlideCollision()?.Collider;
				}
				var distanceFromDialogExpiry = (DialogExpireLocation - Position);

				if (collider != null)
				{
					try
					{
						var rigidBody = (RigidBody2D)collider;
						rigidBody.ApplyCentralImpulse(direction * delta * 1000);
					} 
					catch
					{

					}

					try
					{
						var dialogBody = (DialogBody)collider;
						__dialog.dialog = dialogBody.GetDialog(this);
						DialogExpireLocation = Position;
					}
					catch (Exception ex)
					{
						GD.PushWarning($"Issue creating dialog with collider {ex}");
					}
				}
				else if (animation[0] != sprite.Animation[0] || Math.Abs(distanceFromDialogExpiry.x) > 0 || Math.Abs(distanceFromDialogExpiry.y) > 0)
				{	// clear dialogs if your direction changes
					__dialog.dialog = new DialogText[0];
				}
			}
			if (!Controls.Any) {
				if (sprite.Animation.Contains("walk"))
				{
					animation = $"{sprite.Animation.Split("walk")[0]}default";
				}
			}
			sprite.Animation = animation;
		}
	}
}
