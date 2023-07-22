using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam
{
	public class TalkingToRoomateArea : DialogArea
	{
		private bool once { get; set; }
		public override void NotifyOverlapChange(bool overlap)
		{
			if (overlap && !once && __player.stats.RoommateTrustedYouAndOpenedDoorForYou)
			{
				once = true;
				__player.NewDialog(new[]
				{
					new DialogText
					{
						Name = "You",
						Text = "What . is . that .",
						TextSpeed = 0.100f,
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.Alert);
						}
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "Now, let me stop you right there.",
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.None);
							__player.GetRoommate().FaceDirection("d");
						}
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "I know what you are thinking:"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "Did I invoke powerful dark magic to create a portal"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "behind your back even though I know"
					},
					new DialogText
					{

						Name = "Taylor",
						Text = "we didn't pay the black magic deposit?"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "I mean."
					},
					new DialogText
					{

						Name = "Taylor",
						Text = "I might as well have painted the walls a different color. Right?",
						Options = new Dictionary<string, Func<string>>
						{
							{ "Yes", () => { return "MadAboutDeposit"; } },
							{ "No", () => { return "NoMadAboutDeposit";  } }
						}
					},
					new DialogText
					{
						Id = "MadAboutDeposit",
						Name = "You",
						Text = "That's pretty spot-on to how I am feeling at the moment, yes.",
						Next = "YoudBeRight"
					},
					new DialogText
					{
						Id = "NoMadAboutDeposit",
						Name = "You",
						Text = "What makes this thing so special?",
						Next = "RunOfTheMill"
					},
					new DialogText
					{
						Id = "YoudBeRight",
						Name = "Taylor",
						Text = "Well, you'd be right, except for one small thing."
					},
					new DialogText
					{
						Id = "RunOfTheMill",
						Name = "Taylor",
						Text = "This isn't your regular, run of the mill, black magic portal."
					},
					new DialogText
					{
						Name = "You",
						Text = "How so?"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "We know alternate universes must exist because"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "they sometimes intersect with ours."
					},
					new DialogText
					{
						Name = "You",
						Text = "Obviously. Everybody knows that Taylor."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "Well, then, let me ask you a question then."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "Do universes exist that don't intersect with ours?",
						Options = new Dictionary<string, Func<string>>
						{
							{ "Yes", () => { return "RightAnswer";  } },
							{ "No", () => { return "WrongAnswer";  } }
						}
					}
				});
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
