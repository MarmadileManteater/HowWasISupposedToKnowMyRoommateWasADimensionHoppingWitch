using Godot;
using System;

namespace SummerFediverseJam
{
	public enum BattleOption
	{
		Fight,
		Item,
		Speak,
		None
	}
	// ik parallel arrays are bad form
	public enum MonsterOption
	{
		CupWithMouth,
		SpiderWithFingers,
		MonsterWithEyeball,
		PepperWithLips
	}
	public class Battle : Node2D
	{
		// ik parallel arrays are bad form
		public static Monster[] Bestiary = new[]
		{
			new Monster
			{
				Name = "cup with a mouth",
				ImageName = "cup-with-teeth.png"
			},
			new Monster
			{
				Name = "spider with fingers",
				ImageName = "finger-spider.png"
			},
			new Monster
			{
				Name = "monster with eyeball",
				ImageName = "eyeball-monster.png"
			},
			new Monster
			{
				Name = "pepper with lips",
				ImageName = "pepper-lips.png"
			}
		};
		private AnimatedSprite __monsterSprite;
		private AnimationPlayer __monsterMover;
		private AnimationPlayer __islandMover;
		private AnimationPlayer __phoneMover;
		private AnimationPlayer __fightIcon;
		private AnimationPlayer __itemIcon;
		private AnimationPlayer __speakIcon;
		private AudioStreamPlayer __audioStream;
		private bool __moverSwitch = true;
		[Export]
		public bool moverSwitch
		{
			get
			{
				return __moverSwitch;
			}
			set
			{
				if (__monsterMover != null && __islandMover != null)
				{
					if (value)
					{
						__monsterMover.CurrentAnimation = "Fall down";
						__islandMover.CurrentAnimation = "Fall down";
						__phoneMover.CurrentAnimation = "Fall in";
					}
					else
					{
						__monsterMover.CurrentAnimation = "Fall up";
						__islandMover.CurrentAnimation = "Fall up";
						__phoneMover.CurrentAnimation = "Fall side";
					}

				}
				GD.Print(value);
				__moverSwitch = value;
			}
		}
		private BattleOption __currentOption { get; set; }
		[Export]
		public BattleOption CurrentOption 
		{ 
			get
			{
				return __currentOption;
			}
			set
			{
				if ((int)value > 2)
				{
					value = 0;
				}
				if (value < 0)
				{
					value = (BattleOption)2;
				}
				if (__fightIcon != null)
				{
					switch (value)
					{
						case BattleOption.Fight:
							__fightIcon.CurrentAnimation = "Float";
							__itemIcon.CurrentAnimation = null;
							__speakIcon.CurrentAnimation = null;
							break;
						case BattleOption.Item:
							__fightIcon.CurrentAnimation = null;
							__itemIcon.CurrentAnimation = "Float";
							__speakIcon.CurrentAnimation = null;
							break;
						case BattleOption.Speak:
							__fightIcon.CurrentAnimation = null;
							__itemIcon.CurrentAnimation = null;
							__speakIcon.CurrentAnimation = "Float";
							break;
						case BattleOption.None:
							__fightIcon.CurrentAnimation = null;
							__itemIcon.CurrentAnimation = null;
							__speakIcon.CurrentAnimation = null;
							break;
					}
				}
				__currentOption = value;
			}
		}
		private MonsterOption __currentMonster { get; set; }
		[Export]
		public MonsterOption CurrentMonster
		{
			get
			{
				return __currentMonster;
			}
			set
			{
				if (Bestiary.Length > (int)value && (int)value >= 0)
				{
					var monster = Bestiary[(int)value];
					if (__monsterSprite != null)
						__monsterSprite.Animation = monster.ImageName;
					__currentMonster = value;
				}
			}
		}
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			__audioStream = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
			__monsterSprite = GetNode<AnimatedSprite>("Monster");
			__monsterMover = GetNode<AnimationPlayer>("Monster/Monster Mover");
			__islandMover = GetNode<AnimationPlayer>("FieldOfFloatingIslands/Island Mover");
			__phoneMover = GetNode<AnimationPlayer>("PhoneNode/AnimationPlayer");
			__fightIcon = GetNode<AnimationPlayer>("PhoneNode/Phone Menu/Fight Icon/AnimationPlayer");
			__speakIcon = GetNode<AnimationPlayer>("PhoneNode/Phone Menu/Speak Icon/AnimationPlayer");
			__itemIcon = GetNode<AnimationPlayer>("PhoneNode/Phone Menu/Item Icon/AnimationPlayer");
		}

		public void PlayMusic()
		{
			__audioStream.Play(0);
		}

		public void StopMusic()
		{
			__audioStream.Stop();
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			var player = GetNode<Player>("FieldOfFloatingIslands/Player");
			if (player != null)
			{
				if (@event.IsActionPressed("ui_left"))
				{
					player.ExitBattle();
				}
				if (@event.IsActionPressed("ui_down"))
				{
					CurrentOption++;
				}
				if (@event.IsActionPressed("ui_up"))
				{
					CurrentOption--;
				}
				if (@event.IsActionPressed("ui_accept") && player.IsInBattle)
				{
				}
			}
			base._UnhandledInput(@event);
		}

		public override void _Process(float delta)
		{
			base._Process(delta);
		}

	}
}
