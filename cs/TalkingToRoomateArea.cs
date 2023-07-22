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
						Text = "we didn't pay the dark magic deposit?"
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
						Text = "This isn't your regular, run of the mill, dark magic portal."
					},
					new DialogText
					{
						Name = "You",
						Text = "How so?"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "We know alternate universes must exist because-"
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
						Text = "If a universe falls in the woods and you don't happen to-"
					},
					new DialogText {
						Name = "Taylor",
						Text = "live in a reality that intersects with it on an etherial plane"
					},
					new DialogText {
						Name = "Taylor",
						Text = " does that universe even exist from your perspective?",
						Options = new Dictionary<string, Func<string>>
						{
							{ "Yes", () => { return "RightAnswer";  } },
							{ "No", () => { return "WrongAnswer";  } }
						}
					},
					new DialogText
					{
						Id = "RightAnswer",
						Name = "You",
						Text = "Of course, it exists. You just can't navigate to it-"
					},
					new DialogText
					{
						Name = "You",
						Text = "with your typical, everyday variety of dark magic."
					},
					new DialogText
					{
						Name = "You",
						Text = "But, this isn't typical."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "That is correct.",
						Next = "Continue"
					},
					new DialogText
					{
						Id = "WrongAnswer",
						Name = "Taylor",
						Text = "You are incorrect. Although, that is a common misconception."
					},
					new DialogText
					{
						Id = "Continue",
						Name = "Taylor",
						Text = "To my knowledge, this is the first ever, established-"
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "connection with a parallel universe in human history.",
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.Alert);
						}
					},
					new DialogText
					{
						Name = "You",
						Text = "How did you do it? How can we connect to them-",
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.Wonder);
						}

					},
					new DialogText
					{
						Name = "You",
						Text = "if they are parallel to us? That's the whole point of-"
					},
					new DialogText
					{
						Name = "You",
						Text = "being parallel. Parallel lines don't intersect."
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "Well, maybe, if you follow me through this portal-",
						OnDisplay = () =>
						{
							__player.ShowEmotion(Emotes.None);
						}
					},
					new DialogText
					{
						Name = "Taylor",
						Text = "You just might find out.",
						AfterDequeue = () =>
						{
							__player.GetRoommate().CollisionMask = 10;
							__player.GetRoommate().CollisionLayer = 10;

							__player.GetRoommate().PlayAnimation("WalkUpToPortal");
							__player.GetRoommate().PlayAnimation();
						}
					}
				});
			}
			base.NotifyOverlapChange(overlap);
		}
	}
}
