using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic
{
	public static class Messages
	{
		private static List<string> ClearMessages = new List<string>
		{
			"Everything is groovy",
			"No traffic here",
			"So lonely, come drive around me!",
			"Free and easy!",
			"Now is your chance!",
			"Go around me a few times whilst you can",
			"Hell hath frozen over, traffic is flowing",
			"Dim traffig",
		};

		private static List<string> LightMessages = new List<string>
		{
			"Light traffic",
			"Not too bad",
			"I've seen worse",
			"Meh",
			"As things go, we're not too bad",
		};

		private static List<string> BusyMessages = new List<string>
		{
			"Pretty busy",
			"One does not simply go around the roundabout",
			"Have you considered walking?",
			"I hope you are sitting comfortably",
		};

		private static List<string> VeryBusyMessages = new List<string>
		{
			"Very busy",
			"Might as well have a swift half down the local",
			"I hope you have enough fuel",
			"You shall not pass",
			"Today's delay brought to you by #CCBC",
			"How can one roundabout be so fucked?",
		};

		private static List<string> ExtremelyBusyMessages = new List<string>
		{
			"Stupid busy",
			"Might as well get out and crawl",
			"Abandon hope all ye who enter here",
			"It would be quicker to build a raft and sail the Rhymney River",
			"I'm no traffic scientist, but this is fucked",
			"Where ever you going is almost certainly not worth it",
			"By the time you get where you're going, pigs will be flying",
			"As roundabouts go, I'm pretty fucked",
			"It's busy, so no fighting!",
		};

		private static List<string> CrazyBusyMessages = new List<string>
		{
			"Mental",
			"Go home",
			"I hope you packed a tent",
			"Give up...",
		};

		private static List<string> UnpassableMessages = new List<string>
		{
			"The roads are totally and utterly fucked, don't even look at your car!",
			"Go home, it's not worth it",
			"It's like a scene from #TheWalkingDead",
			"It might be time to consider moving to another country"
		};

		private static string HashTags = "#Pwllypant #Caerphilly";

		public static string SelectMessage(int result, out string hashTags)
		{
			Random rand = new Random();

			if (result <= 2)
			{
				hashTags = HashTags;
				return ClearMessages[rand.Next(ClearMessages.Count)];
			}

			else if (result > 2 && result < 7)
			{
				hashTags = HashTags;
				return LightMessages[rand.Next(LightMessages.Count)];
			}

			else if (result >= 7 && result < 15)
			{
				hashTags = HashTags;
				return BusyMessages[rand.Next(BusyMessages.Count)];
			}

			else if (result >= 15 && result < 20)
			{
				hashTags = HashTags;
				return VeryBusyMessages[rand.Next(VeryBusyMessages.Count)];
			}

			else if (result >= 20 && result < 30)
			{
				hashTags = HashTags;
				return ExtremelyBusyMessages[rand.Next(ExtremelyBusyMessages.Count)];
			}

			else if (result >= 30 && result < 45)
			{
				hashTags = HashTags;
				return CrazyBusyMessages[rand.Next(CrazyBusyMessages.Count)];
			}

			else
			{
				hashTags = HashTags;
				return UnpassableMessages[rand.Next(UnpassableMessages.Count)];
			}
		}

		public static string FormatMessage(string message, string destination, string hashTags, int result, DateTime currentTime)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(message);

			if (result <= 2)
				sb.Append(String.Format(" - {0} bound at {1} {2}", destination, currentTime.ToString("dd/MM/yyyy HH:mm:ss"), hashTags));
			else
				sb.Append(String.Format(" - {0}mins delay, {1} bound at {2} {3}", result, destination, currentTime.ToString("dd/MM/yyyy HH:mm:ss"), hashTags));
			return sb.ToString();
		}
	}
}
