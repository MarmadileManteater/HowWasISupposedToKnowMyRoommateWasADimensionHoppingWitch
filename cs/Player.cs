using Godot;
using SummerFediverseJam.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace SummerFediverseJam {
	public enum Emotes
	{
		Alert,
		Love,
		Wonder,
		None
	}
	public class Player : KinematicBody2D
	{
		public GameStats stats = new GameStats { HP = 10, MonstersVanquished = new List<MonsterOption>(), MonstersAquianted = new List<MonsterOption>() };
		public Node2D root;
		private Dialog __dialog;
		private Battle __battleScene;
		private AudioStreamPlayer __backgroundMusic;
		private AudioStreamPlayer __unlockSoundEffect;
		private AudioStreamPlayer __achievedSoundEffect;
		private AnimatedSprite __emotes;
		private AnimationPlayer __dimensionAnimation;
		private ColorRect __apartmentMask;
		private AnimationPlayer __apartmentMaskAnimationPlayer;
		private Roommate __roommate;
		private BadRoommateEndingCard __badEndCard;
		private ColorRect[] __sky;
		private Vector2 DialogExpireLocation = new Vector2(0, 0);
		public Controls Controls = new Controls();
		private Vector2 PrebattlePosition = new Vector2(0, 0);
		public bool GameOver = false;
		public bool IsInBattle = false;
		public bool IsTransitionCurrentlyHappening = false;
		private Node2D ParentBeforeBattle;
		public bool TouchEnabled = false;
		private bool UsingMouse;
		public bool HasDialog
		{
			get
			{
				return __dialog.dialog.Length > 0 && __dialog.PhraseNum > -1;
			}
		}
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
			root = GetParent<Node2D>();
			__dialog = GetNode<Dialog>("camera/Dialog");
			__dialog.dialog = new DialogText[0];
			__battleScene = GetParent().GetNode<Battle>("Battle");
			__backgroundMusic = GetParent().GetNode<AudioStreamPlayer>("AudioStreamPlayer");
			__unlockSoundEffect = GetParent().GetNode<AudioStreamPlayer>("UnlockSoundEffect");
			__achievedSoundEffect = GetParent().GetNode<AudioStreamPlayer>("AchievedSoundEffect");
			__emotes = GetNode<AnimatedSprite>("Emotes");
			__dimensionAnimation = root.GetNode<AnimationPlayer>("DimensionAnimation");
			__apartmentMask = root.GetNode<ColorRect>("ApartmentMask");
			__apartmentMaskAnimationPlayer = __apartmentMask.GetNode<AnimationPlayer>("AnimationPlayer");
			__roommate = root.GetNode<Roommate>("Roommate character");
			__badEndCard = GetNode<BadRoommateEndingCard>("BadRoommateEnding2");
			__sky = new ColorRect[]
			{
				root.GetNode<ColorRect>("Sky"),
				root.GetNode<ColorRect>("Sky2"),
				root.GetNode<ColorRect>("Sky3")
			};

		}

		public void SetSkyColor(Color color)
		{
			foreach (var rect in __sky)
			{
				rect.Color = color;
			}
		}

		public void ShowBadEndCard()
		{
			__badEndCard.SetScore(stats.Score);
			__badEndCard.Show();
		}

		public void ShowEndCard(string message)
		{
			// i'm just reusing this, need to fix names at some point
			__badEndCard.SetScore(stats.Score);
			__badEndCard.SetCardTitle(message);
			__badEndCard.Show();
		}

		public Roommate GetRoommate()
		{
			return __roommate;
		}

		public void FadeOutAptMask()
		{
            __apartmentMaskAnimationPlayer.CurrentAnimation = "Fade out";
		}
		public void FadeInAptMask()
		{
			__apartmentMaskAnimationPlayer.CurrentAnimation = "Fade in";
		}

		public void ShowAptMask(int alpha = 255)
		{
			__apartmentMask.Color = new Color(__apartmentMask.Color.r, __apartmentMask.Color.g, __apartmentMask.Color.b, alpha);
			__apartmentMask.Show();
		}
		public void HideAptMask()
		{
			__apartmentMask.Hide();
		}

		public void CollapseDimension()
		{
			__dimensionAnimation.CurrentAnimation = "collapse";
		}

		public void ExpandDimension()
		{
			__dimensionAnimation.CurrentAnimation = "expand";
		}

		public void PauseBackgroundMusic()
		{
			__backgroundMusic.Stop();
		}

		public void PlayBackgroundMusic()
		{
			__backgroundMusic.Play(0);
		}

		public void ShowEmotion(Emotes emote)
		{
			switch(emote)
			{
				case Emotes.Alert:
					__emotes.Animation = "alert";
					break;
				case Emotes.Love:
					__emotes.Animation = "love";
					break;
				case Emotes.Wonder:
					__emotes.Animation = "wonder";
					break;
				default:
					__emotes.Animation = "default";
					break;
			}
		}

		public void FaceDirection(string direction)
		{
			GetNode<AnimatedSprite>("character").Animation = $"{direction}default";
		}

		public void PlayUnlockJingle()
		{
			__unlockSoundEffect.Play();
		}
		
		public void PlayAchievedJingle()
		{
			__achievedSoundEffect.Play();
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
			ParentBeforeBattle = GetParent<Node2D>();
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
			if (!IsInBattle)
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
				if (GetParent() != root)
				{
					GetParent().GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
				}
				__dialog.Scale *= 3;
				__dialog.Position *= 3;
				__battleScene.PlayMusic();
				timer.Connect("timeout", this, nameof(EnterBattleCallback));
				IsInBattle = true;
			}
		}

		public void ExitBattle()
		{
			if (IsInBattle)
			{
				GetParent().RemoveChild(this);
				var camera = __battleScene.GetNode<Camera2D>("camera");
				__battleScene.RemoveChild(camera);
				camera.Position = new Vector2(0, 0);
				AddChild(camera);
				Scale = new Vector2(3, 3);
				Position = PrebattlePosition;
				if (ParentBeforeBattle == root)
				{
					root.AddChildBelowNode(root.GetNode<TileMap>("Environment layer 2"), this);
					__backgroundMusic.Play(0);
				} else
				{
					ParentBeforeBattle.GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
					ParentBeforeBattle.AddChild(this);
				}
				CollisionLayer = 1;
				CollisionMask = 1;
				__battleScene.StopMusic();
				var player = GetNode<AnimationPlayer>("Node2D/BattleTransitionMask/AnimationPlayer");
				player.CurrentAnimation = "fade-in";
				IsInBattle = false;

				__dialog.Scale = new Vector2(1, 1);
				__dialog.Position /= 3;
				__battleScene.Hide();
			}
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
			if (@event.IsActionPressed("ui_accept") && !__badEndCard.Visible)
			{
				__dialog.NextPhrase();
			}
			if (@event.IsActionPressed("ui_accept") && __badEndCard.Visible) {
				GetTree().ReloadCurrentScene();
			}
			base._UnhandledInput(@event);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			if (Input.IsMouseButtonPressed(1)) {
				if (!TouchEnabled)
				{
					Controls.Left = GetViewport().GetMousePosition().x + (GetViewport().Size.x / 10) < GetViewport().Size.x / 2;
					Controls.Right = GetViewport().GetMousePosition().x - (GetViewport().Size.x / 10) > GetViewport().Size.x / 2;
					Controls.Up = GetViewport().GetMousePosition().y + (GetViewport().Size.y / 10) < GetViewport().Size.y / 2;
					Controls.Down = GetViewport().GetMousePosition().y - (GetViewport().Size.y / 10) > GetViewport().Size.y / 2;
				}
				if (!UsingMouse && __badEndCard.Visible)
				{
					GetTree().ReloadCurrentScene();
				}
				if (!UsingMouse && !__badEndCard.Visible && ((!Controls.Any && TouchEnabled) || !TouchEnabled))
				{
					__dialog.NextPhrase(true);
				}
				UsingMouse = true;
			} else if (UsingMouse) {
				Controls.Up = false;
				Controls.Down = false;
				Controls.Left = false;
				Controls.Right = false;
				UsingMouse = false;
			}
			var sprite = GetNode<AnimatedSprite>("character");
			// only want to set animation once per `_Process`
			// otherwise the animation won't play if it keeps flipping back and forth before the animation plays
			var animation = sprite.Animation;
			// only activate controls if the dialog is closed or unpopulated
			if (__dialog.PhraseNum == -1 && !(__battleScene.moverSwitch == true && CollisionLayer == 2) && root.GetNode<MainMenu>("TitleCard").isPlaying && !GameOver) {
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
				if (direction != Vector2.Zero && root.Scale.x == 1)
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

		private void _on_Right_gui_input(object @event)
		{
			if (@event is InputEventScreenTouch touchEvent)
			{
				Controls.Right = touchEvent.Pressed;
			}
		}

		private void _on_Left_gui_input(object @event)
		{
			if (@event is InputEventScreenTouch touchEvent)
			{
				Controls.Left = touchEvent.Pressed;
			}
		}


		private void _on_Up_gui_input(object @event)
		{
			if (@event is InputEventScreenTouch touchEvent)
			{
				Controls.Up = touchEvent.Pressed;
			}
		}

		private void _on_Down_gui_input(object @event)
		{
			if (@event is InputEventScreenTouch touchEvent)
			{
				Controls.Down = touchEvent.Pressed;
			}
		}


	}
}
