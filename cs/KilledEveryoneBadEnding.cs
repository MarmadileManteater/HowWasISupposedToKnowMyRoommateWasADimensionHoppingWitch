using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class KilledEveryoneBadEnding : DialogArea
	{
		private AnimatedSprite __ada;
		private Timer __timer;
		public override void _Ready()
		{
			__ada = GetNode<AnimatedSprite>("GuardianOfTheForest");
			__timer = GetNode<Timer>("Timer");
			base._Ready();
		}

		public void GameOver()
		{
			if (__player.stats.MonstersVanquished.Count == 4)
			{
				__player.ShowEndCard("The \"I'm the villian?\" Ending");
				__player.GameOver = true;
				__timer.Stop();
			} else if (__player.stats.MonstersAquianted.Count == 4) {
                __player.ShowEndCard("The Good Ending");
                __player.GameOver = true;
                __timer.Stop();
            } else {
                __player.ShowEndCard("The Neutral Ending");
                __player.GameOver = true;
                __timer.Stop();
            }
		}

		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap)
			{
				if (__player.stats.MonstersVanquished.Count == 4)
				{
					DisableCheck = true;
					if (!__player.stats.IsRoommateInParty)
					{
						__player.NewDialog(new[]
						{
						new DialogText
						{
							Name = "Ada",
							Text = "What are you doing here exactly?",
							OnDisplay = () =>
							{
								__ada.Animation = "left";
							}
						},
						new DialogText
						{
							Name = "You",
							Text = "I vanquished all of the monsters in this forest."
						},
						new DialogText
						{
							Name = "Ada",
							Text = "Vanquished? Monsters? You murdered all our residents!",
							TextSpeed = 0.05f
						},
						new DialogText
						{
							Name = "Ada",
							Text = "Did you even know their names when you slaughtered them?"
						},
						new DialogText
						{
							Name = "You",
							Text = "Oh no. Am I actually the villian?",
                            AfterDequeue = () =>
                            {
                                __player.GameOver = true;
                                __timer.WaitTime = 1;
                                __timer.Start();
                                __timer.Connect("timeout", this, nameof(GameOver));
                            }
                        }
					});
				} else {
						__player.NewDialog(new[]
						{
							new DialogText
							{
								Name = "Ada",
								Text = "What are y'all doing here exactly?",
								OnDisplay = () =>
								{
									__ada.Animation = "left";
								}
							},
							new DialogText
							{
								Name = "Taylor",
								Text = "We vanquished all of the monsters in this forest."
                            },
							new DialogText
							{
								Name = "Ada",
								Text = "Vanquished? Monsters? You murdered all our residents!"
							},
							new DialogText
							{
								Name = "Ada",
								Text = "Did you even know their names when you slaughtered them?"
							},
							new DialogText
							{
								Name = "You",
								Text = "Uhhhhh."
							},
							new DialogText
							{
								Name = "Taylor",
								Text = "Dang, are we the bad guys?",
								AfterDequeue = () =>
								{
									__player.GameOver = true;
									__timer.WaitTime = 1;
									__timer.Start();
									__timer.Connect("timeout", this, nameof(GameOver));
								}
							}
						});
					}
				} else if (__player.stats.MonstersOutOfCirculation == 4) {
                    DisableCheck = true;
                    if (!__player.stats.IsRoommateInParty)
                    {
                        __player.NewDialog(new[]
                        {
							new DialogText
							{
								Name = "Ada",
								Text = "What are you doing here exactly?",
								OnDisplay = () =>
								{
									__ada.Animation = "left";
								}
							},
							new DialogText
							{
								Name = "You",
								Text = "I am from a parallel universe. I come in peace."
							},
							new DialogText
							{
								Name = "Ada",
								Text = "Yeah, peace. I totally believe you. ~rolls eyes~",
								TextSpeed = 0.05f
							},
							new DialogText
							{
								Name = "Ada",
								Text = "Please, leave my universe now, or face the consequences."
							},
							new DialogText
							{
								Name = "You",
								Text = "uhhh, okay, I guess it was nice while it lasted.",
                                AfterDequeue = () =>
                                {
                                    __player.GameOver = true;
                                    __timer.WaitTime = 1;
                                    __timer.Start();
                                    __timer.Connect("timeout", this, nameof(GameOver));
                                }
                            }
						});
                    }
                    else
                    {
                        __player.NewDialog(new[]
                        {
                           new DialogText
                            {
                                Name = "Ada",
                                Text = "What are you doing here exactly?",
                                OnDisplay = () =>
                                {
                                    __ada.Animation = "left";
                                }
                            },
                            new DialogText
                            {
                                Name = "Taylor",
                                Text = "We are from a parallel universe. We come in peace."
                            },
                            new DialogText
                            {
                                Name = "Ada",
                                Text = "Yeah, peace. I totally believe you. ~rolls eyes~",
                                TextSpeed = 0.05f
                            },
                            new DialogText
                            {
                                Name = "Ada",
                                Text = "Please, leave my universe now, or face the consequences."
                            },
                            new DialogText
                            {
                                Name = "Taylor",
                                Text = "But, we'll be back though."
                            },
                            new DialogText
                            {
                                Name = "Ada",
                                Text = "Please."
                            },
                            new DialogText
                            {
                                Name = "Ada",
                                Text = "Don't.",
                                AfterDequeue = () =>
                                {
                                    __player.GameOver = true;
                                    __timer.WaitTime = 1;
                                    __timer.Start();
                                    __timer.Connect("timeout", this, nameof(GameOver));
                                }
                            },
                        });
                    }
                }
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
