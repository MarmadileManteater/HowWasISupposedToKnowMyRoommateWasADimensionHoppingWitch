using Godot;

using System;
using System.Collections.Generic;

namespace SummerFediverseJam {
	public class Player : KinematicBody2D
	{
		private Node __root;
		private Dialog __dialog;
		private Battle __battleScene;
		private AudioStreamPlayer __backgroundMusic;
		private Vector2 DialogExpireLocation = new Vector2(0, 0);
		public Controls Controls = new Controls();
		private Vector2 PrebattlePosition = new Vector2(0, 0);
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
			__root = GetParent();
			__dialog = GetNode<Dialog>("Dialog");
			__dialog.dialog = new DialogText[0];
			__battleScene = GetParent().GetNode<Battle>("Battle");
			__backgroundMusic = GetParent().GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		}
		
		public void NewDialog(DialogText[] dialog)
		{
			__dialog.dialog = dialog;
			__dialog.NextPhrase();
		}

		public void EnterBattleCallback()
		{
            var mask = GetNode<ColorRect>("Node2D/BattleTransitionMask");
            GetNode<Timer>("Node2D/BattleTransitionTimer").Stop();
            GetParent().RemoveChild(this);
            __battleScene.GetNode<TileMap>("FieldOfFloatingIslands").AddChild(this);
            var camera = GetNode<Camera2D>("camera");
            RemoveChild(camera);
            camera.Position = new Vector2(500, 300);
            PrebattlePosition = Position;
            Position = new Vector2(167, 128);
            Scale = new Vector2(1, 1);
            __battleScene.AddChild(camera);
            __battleScene.Show();
            CollisionLayer = 2;
            CollisionMask = 2;
			mask.Hide();
        }

		// todo ✏ add param for monster information
		public void EnterBattle(MonsterOption option)
		{
            var mask = GetNode<ColorRect>("Node2D/BattleTransitionMask");
            var player = GetNode<AnimationPlayer>("Node2D/BattleTransitionMask/AnimationPlayer");
            mask.Show();
            var timer = GetNode<Timer>("Node2D/BattleTransitionTimer");
            player.CurrentAnimation = "fade-out";
            __battleScene.CurrentMonster = option;
            __battleScene.CurrentOption = BattleOption.None;
            __battleScene.CurrentOption = BattleOption.Fight;
			timer.WaitTime = 2.5f;
			timer.Start();
			__backgroundMusic.Stop();
            __battleScene.PlayMusic();
            timer.Connect("timeout", this, nameof(EnterBattleCallback));
        }

		public void ExitBattle()
		{
			GetParent().RemoveChild(this);
			var camera = __battleScene.GetNode<Camera2D>("camera");
			__battleScene.RemoveChild(camera);
			camera.Position = new Vector2(0, 0);
			AddChild(camera);
			Scale = new Vector2(3, 3);
			Position = PrebattlePosition;
			__root.AddChildBelowNode(__root.GetNode<TileMap>("Environment layer 2"), this);
			CollisionLayer = 1;
			CollisionMask = 1;
			__battleScene.StopMusic();
            var player = GetNode<AnimationPlayer>("Node2D/BattleTransitionMask/AnimationPlayer");
			player.CurrentAnimation = "fade-in";
			__backgroundMusic.Play(0);
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
			if (@event.IsActionPressed("ui_accept") && !(__battleScene.moverSwitch == true && CollisionLayer == 2))
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
			if (__dialog.PhraseNum == -1 && !(__battleScene.moverSwitch == true && CollisionLayer == 2)) {
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
						//GD.PushWarning($"Issue creating dialog with collider {ex}");
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
