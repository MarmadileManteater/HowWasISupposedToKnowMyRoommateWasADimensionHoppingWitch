using Godot;
using System;
using System.Collections.Generic;

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
		public static Monster[] Bestiary;
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
		private Random rng = new Random();
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
				if ((int)value > 3)
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
			Bestiary = new[]
			{
				new Monster
				{
					Name = "cup with a mouth",
					ImageName = "cup-with-teeth.png",
					ChanceToDie = 100,
					DamageDone = 0,
					Speak = new []
					{
						new DialogText
						{
							Name = "You",
							Text = "uh, hello, how are you doing?"
						},
						new DialogText
						{
							Text = "~the cup with teeth fled the encounter~",
							OnDisplay = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									player.stats.MonstersAquianted.Add(MonsterOption.CupWithMouth);
									__monsterMover.CurrentAnimation = "Flee";
								}
							},
							AfterDequeue = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									if (player.stats.MonstersOutOfCirculation == 4)
									{
										player.root.GetNode<Node2D>("Parallel Dimension/SemiBadEnding").Show();
									}
									player.ExitBattle();
									__monsterMover.CurrentAnimation = "StayStill";
								}
							}
						}
					}
				},
				new Monster
				{
					Name = "spider with fingers",
					ImageName = "finger-spider.png",
					ChanceToDie = 40,
					DamageDone = 2,
					Speak = new []
					{
						new DialogText
						{
							Name = "You",
							Text = "Do you have any hobbies?"
						},
						new DialogText
						{
							Text = "~the spider begins to tap rhythmically~",
						},
						new DialogText
						{
							Text = "~you don't know how you know this, but~",
						},
						new DialogText
						{
							Text = "~this spider would be great at ASMR~",
							Options = new System.Collections.Generic.Dictionary<string, Func<string>>
							{
								{ "Ask", () => { return "Ask"; } },
								{ "Jeer", () => { return "Jeer";  } }
							}
						},
						new DialogText
						{
							Id = "Ask",
							Name = "You",
							Text = "Do you have any interest in ASMR?"
						},
						new DialogText
						{
							Text = "~the spider taps exictedly~"
						},
						new DialogText
						{
							Text = "~the spider gives you their PeerTube handle~",
							OnDisplay = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									player.PlayAchievedJingle();
								}
							}
						},
						new DialogText
						{
							Text = "~you feel pretty good about that interaction~",
							OnDisplay = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									player.stats.GoodPersonIndex++;
									player.stats.MonstersAquianted.Add(MonsterOption.SpiderWithFingers);
									__monsterMover.CurrentAnimation = "Flee";
								}
							},
							AfterDequeue = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									if (player.stats.MonstersOutOfCirculation == 4)
									{
										player.root.GetNode<Node2D>("Parallel Dimension/SemiBadEnding").Show();
									}
									player.ExitBattle();
									__monsterMover.CurrentAnimation = "StayStill";
								}
							},
							End = true
						},
						new DialogText
						{
							Id = "Jeer",
							Name = "You",
							Text = "You call that ASMR? I couldn't relax to your-"
						},
						new DialogText
						{
							Name = "You",
							Text = "-creepy-self in a million billion years."
						},
						new DialogText
						{
							Text = "~the spider cries as it runs away~",
							OnDisplay = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									player.stats.GoodPersonIndex--;
									player.stats.MonstersAquianted.Add(MonsterOption.SpiderWithFingers);
									__monsterMover.CurrentAnimation = "Flee";
								}
							},
							AfterDequeue = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									if (player.stats.MonstersOutOfCirculation == 4)
									{
										player.root.GetNode<Node2D>("Parallel Dimension/SemiBadEnding").Show();
									}
									player.ExitBattle();
									__monsterMover.CurrentAnimation = "StayStill";
								}
							},
							End = true

						}
					}
				},
				new Monster
				{
					Name = "monster with eyeball",
					ImageName = "eyeball-monster.png",
					ChanceToDie = 70,
					DamageDone = 4,
					Speak = new []
					{
						new DialogText
						{
							Name = "You",
							Text = "Have you seen any good movies lately?"
						},
						new DialogText
						{
							Text = "~the monster blushes and remarks about-"
						},
						new DialogText
						{
							Text = "-how seeing things is one of their-"
						},
						new DialogText
						{
							Text = "-favorite past times~",
							Options = new Dictionary<string, Func<string>>
							{
								{ "Me too", () => { return "MeToo"; } },
								{ "Okay?", () => { return "EmbarressedMonster"; } }
							}
						},
						new DialogText
						{
							Id = "MeToo",
							Text = "~the monster remarks about how you should-"
						},
						new DialogText
						{
							Text ="-subscribe to their lemmy community called-"
						},
						new DialogText
						{
							Text = "-Visual Enthusiasts~"
						},
						new DialogText
						{
							Text = "(you both continue to talk for quite some time)"
						},
						new DialogText
						{
							Text = "(eventually, Antonio, has to go somewhere and leaves.)",
							OnDisplay = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									player.stats.GoodPersonIndex++;
									player.stats.MonstersAquianted.Add(MonsterOption.MonsterWithEyeball);
									__monsterMover.CurrentAnimation = "Flee";
								}
							},
							AfterDequeue = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									if (player.stats.MonstersOutOfCirculation == 4)
									{
										player.root.GetNode<Node2D>("Parallel Dimension/SemiBadEnding").Show();
									}
									player.ExitBattle();
									__monsterMover.CurrentAnimation = "StayStill";
								}
							},
							End = true
						},
						new DialogText
						{
							Id = "EmbarressedMonster",
							Text = "~the monster seems embarrased~",
							OnDisplay = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									player.stats.GoodPersonIndex--;
									player.stats.MonstersAquianted.Add(MonsterOption.MonsterWithEyeball);
									__monsterMover.CurrentAnimation = "Flee";
								}
							},
							AfterDequeue = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									if (player.stats.MonstersOutOfCirculation == 4)
									{
										player.root.GetNode<Node2D>("Parallel Dimension/SemiBadEnding").Show();
									}
									player.ExitBattle();
									__monsterMover.CurrentAnimation = "StayStill";
								}
							},
							End = true
						},
					}
				},
				new Monster
				{
					Name = "pepper with lips",
					ImageName = "pepper-lips.png",
					ChanceToDie = 80,
					DamageDone = 6,
					Speak = new []
					{
						new DialogText
						{
							Name = "You",
							Text = "Hello! You have pretty lips~!"
						},
						new DialogText
						{
							Text = "~the pepper seems flustered~"
						},
						new DialogText
						{
							Name = "Pete",
							Text = "You don't really mean that!",
							Options = new Dictionary<string, Func<string>>
							{
								{ "I do", () => { return "BlushingPepper"; } },
								{ "I don't", () => { return "HurtPepper"; } }
							}
						},
						new DialogText
						{
							Id ="BlushingPepper",
							Name = "Pete",
							Text = "Oh my goodness. You are embarassing me~!",
							OnDisplay = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									player.stats.GoodPersonIndex++;
									player.stats.MonstersAquianted.Add(MonsterOption.PepperWithLips);
									__monsterMover.CurrentAnimation = "Flee";
								}
							},
							AfterDequeue = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									if (player.stats.MonstersOutOfCirculation == 4)
									{
										player.root.GetNode<Node2D>("Parallel Dimension/SemiBadEnding").Show();
									}
									player.ExitBattle();
									__monsterMover.CurrentAnimation = "StayStill";
								}
							},
							End = true
						},
						new DialogText
						{
							Id = "HurtPepper",
							Name = "Pete",
							Text = "Ouch! Tell me how you really feel, why don't ya!",
							OnDisplay = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									player.stats.GoodPersonIndex--;
									player.stats.MonstersAquianted.Add(MonsterOption.PepperWithLips);
									__monsterMover.CurrentAnimation = "Flee";
								}
							},
							AfterDequeue = () =>
							{
								if (HasNode("FieldOfFloatingIslands/Player")) {
									var player = GetNode<Player>("FieldOfFloatingIslands/Player");
									if (player.stats.MonstersOutOfCirculation == 4)
									{
										player.root.GetNode<Node2D>("Parallel Dimension/SemiBadEnding").Show();
									}
									player.ExitBattle();
									__monsterMover.CurrentAnimation = "StayStill";
								}
							},
							End = true
						}
					}
				}
			};
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

		public void GameoverTimeout()
		{
			if (HasNode("FieldOfFloatingIslands/Player"))
			{
				var player = GetNode<Player>("FieldOfFloatingIslands/Player");
				player.ExitBattle();
				player.ShowEndCard("You died.");
			}
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
							if (CurrentOption == BattleOption.Speak)
							{
								if (Bestiary[(int)CurrentMonster].Speak != null)
								{
									CurrentOption = BattleOption.None;
									player.NewDialog(Bestiary[(int)CurrentMonster].Speak);
								}
							}
							if (CurrentOption == BattleOption.Fight)
							{
								CurrentOption = BattleOption.None;
								if (rng.Next(0, 100) < Bestiary[(int)CurrentMonster].ChanceToDie)
								{
									__flash.Play("Flash");
									// ouchie monster is dead 
									player.NewDialog(new[]
									{
										new DialogText
										{
											Text = "You attacked the foe."
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
												if (player.stats.MonstersOutOfCirculation == 4)
												{
													player.root.GetNode<Node2D>("Parallel Dimension/SemiBadEnding").Show();
												}
												player.ExitBattle();
												__monsterMover.CurrentAnimation = "StayStill";
											}
										}
									});
								} else {
									player.stats.HP -= Bestiary[(int)CurrentMonster].DamageDone;
									__shake.CurrentAnimation = "Shake";
									__shake.Play();
									if (player.stats.HP <= 0)
									{
										player.GameOver = true;
									}
									player.NewDialog(new[] {
										new DialogText
										{
											Text = "~you missed~"
										},
										new DialogText
										{
											Text = $"~you took {Bestiary[(int)CurrentMonster].DamageDone} points of damage~",
											AfterDequeue = () =>
											{
												CurrentOption = BattleOption.None;
												if (player.stats.HP <= 0)
												{
													player.GameOver = true;
													var timer = GetNode<Timer>("Timer");
													timer.WaitTime = 1;
													timer.Connect("timeout", this, nameof(GameoverTimeout));
													timer.Start();
													GetNode<AnimationPlayer>("BattleFade/BattleFadeOut/AnimationPlayer").CurrentAnimation = "FadeOut";
												}
											},
											End = player.stats.HP > 0
										},
										new DialogText
										{
											Text = "You died."
										}
									});
								}

							}
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
												if (player.stats.MonstersOutOfCirculation == 4)
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

