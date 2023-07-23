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
		private AnimationPlayer __flash;
		private AnimationPlayer __shake;
		private AudioStreamPlayer __magicSound;
		private RichTextLabel __hp;
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
			__hp = GetNode<RichTextLabel>("PhoneNode/Phone Menu/HP");
			__flash = GetNode<AnimationPlayer>("Flash/ColorRect/AnimationPlayer");
			__shake = GetNode<AnimationPlayer>("AnimationPlayer");
			__magicSound = GetNode<AudioStreamPlayer>("Flash/AudioStreamPlayer");
			__monsterMover.CurrentAnimation = "StayStill";
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
			if (HasNode("FieldOfFloatingIslands/Player")) {
				var player = GetNode<Player>("FieldOfFloatingIslands/Player");
				if (player != null)
				{
					if (moverSwitch && !player.HasDialog) {
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
							if (CurrentOption == BattleOption.Item)
							{
								CurrentOption = BattleOption.None;
								if (!player.stats.HasGun)
								{
									player.NewDialog(new[] {
										new DialogText
										{
											Text = "You don't have any items."
										}
									});
								} else {
									__flash.Play("Flash");
									__magicSound.Play();
									player.NewDialog(new[]
									{
										new DialogText
										{
											Text = "You used your laser gun."
										},
										new DialogText
										{
											Text = "The foe was defeated!",
											OnDisplay = () =>
											{
												__monsterMover.CurrentAnimation = "Fade out";
											},
											AfterDequeue = () =>
											{
												player.stats.MonstersVanquished.Add(CurrentMonster);
												if (player.stats.MonstersVanquished.Count == 4)
												{
													player.root.GetNode<Node2D>("Parallel Dimension/SemiBadEnding").Show();
												}
												player.ExitBattle();
												__monsterMover.CurrentAnimation = "StayStill";
											}
										}
									});
								}
							}
						}
					}
				}
			}
			base._UnhandledInput(@event);
		}

		public override void _Process(float delta)
		{
			if (HasNode("FieldOfFloatingIslands/Player"))
			{
				var player = GetNode<Player>("FieldOfFloatingIslands/Player");
				GD.Print(player.stats.HP);
				GD.Print($"HP ${player.stats.HP}/10");
				__hp.Text = $"HP {player.stats.HP}/10";
				if (player.HasDialog) {
					GetNode<Node2D>("PhoneNode").Hide();
				} else {
					GetNode<Node2D>("PhoneNode").Show();
				}
			}
			base._Process(delta);
		}

	}
}
