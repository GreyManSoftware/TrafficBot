using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterTraffic
{
	public abstract class Messages
	{
		public virtual List<string> ClearMessages => new List<string>
		{
			"Everything is groovy",
			"No traffic here",
			"Free and easy!",
			"Now is your chance!",
			"Hell hath frozen over, traffic is flowing"
		};

		public virtual List<string> LightMessages => new List<string>
		{
			"Light traffic",
			"Not too bad",
			"I've seen worse",
			"Meh",
		};

		public virtual List<string> BusyMessages => new List<string>
		{
			"Pretty busy",
			"Have you considered walking?",
			"I hope you are sitting comfortably",
			"It has gone to shit",
		};

		public virtual List<string> VeryBusyMessages => new List<string>
		{
			"Very busy",
			"Might as well have a swift half down the local",
			"I hope you have enough fuel",
			"You shall not pass"
		};

		public virtual List<string> ExtremelyBusyMessages => new List<string>
		{
			"Stupid busy",
			"Might as well get out and crawl",
			"Abandon hope all ye who enter here",
			"I'm no traffic scientist, but this is fucked",
			"Where ever you going is almost certainly not worth it",
			"By the time you get where you're going, pigs will be flying",
			"It's busy, so no fighting!"
		};

		public virtual List<string> CrazyBusyMessages => new List<string>
		{
			"Mental",
			"Go home",
			"I hope you packed a tent",
			"Give up..."
		};

		public virtual List<string> UnpassableMessages => new List<string>
		{
			"The roads are totally and utterly fucked, don't even look at your car!",
			"Go home, it's not worth it",
			"It's like a scene from #TheWalkingDead",
			"It might be time to consider moving to another country"
		};

		public virtual string HashTags => "#TrafficBot";
		protected Queue<string> MessageCache = new Queue<string>(3);

		public bool RecentlyUsed(string message)
		{
			if (MessageCache.Contains(message))
			{
				return true;
			}

			MessageCache.Enqueue(message);

			if (MessageCache.Count > 3)
				MessageCache.Dequeue();

			return false;
		}

		public string SelectMessage(int result)
		{
			Random rand = new Random();

			if (result <= 2)
			{
				string message = ClearMessages[rand.Next(ClearMessages.Count)];

				if (RecentlyUsed(message))
					message = ClearMessages[rand.Next(ClearMessages.Count)];

				return message;
			}

			else if (result > 2 && result < 7)
			{
				string message = LightMessages[rand.Next(LightMessages.Count)];

				if (RecentlyUsed(message))
					message = LightMessages[rand.Next(LightMessages.Count)];

				return message;
			}
			else if (result >= 7 && result < 15)
			{
				string message = BusyMessages[rand.Next(BusyMessages.Count)];

				if (RecentlyUsed(message))
					message = BusyMessages[rand.Next(BusyMessages.Count)];

				return message;
			}

			else if (result >= 15 && result < 20)
			{
				string message = VeryBusyMessages[rand.Next(VeryBusyMessages.Count)];

				if (RecentlyUsed(message))
					message = VeryBusyMessages[rand.Next(VeryBusyMessages.Count)];

				return message;
			}

			else if (result >= 20 && result < 30)
			{
				string message = ExtremelyBusyMessages[rand.Next(ExtremelyBusyMessages.Count)];

				if (RecentlyUsed(message))
					message = ExtremelyBusyMessages[rand.Next(ExtremelyBusyMessages.Count)];

				return message;
			}

			else if (result >= 30 && result < 45)
			{
				string message = CrazyBusyMessages[rand.Next(CrazyBusyMessages.Count)];

				if (RecentlyUsed(message))
					message = CrazyBusyMessages[rand.Next(CrazyBusyMessages.Count)];

				return message;
			}

			else
			{
				string message = UnpassableMessages[rand.Next(UnpassableMessages.Count)];

				if (RecentlyUsed(message))
					message = UnpassableMessages[rand.Next(UnpassableMessages.Count)];

				return message;
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
