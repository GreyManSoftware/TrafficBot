using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterTraffic.Bots;

namespace TwitterTraffic
{
	public class Twitter
	{
		private TwitterContext TwitterConnection;

		public async void PostTweet(string message)
		{
			try
			{
				await TwitterConnection.TweetAsync(message);
				Console.WriteLine("Updated status successfully");
			}
			catch
			{
				Console.WriteLine("Status hasn't changed");
			}
		}

		public DateTime GetLastTweet()
		{
			Status lastTweet = TwitterConnection.Status.Where(s => s.Type == StatusType.User && s.ScreenName == "PwllypantRound" && s.InReplyToScreenName == null).FirstOrDefault();
			//Console.WriteLine(lastTweet.Text);
			if (lastTweet != null)
				return Utils.GetLocalTime(lastTweet.CreatedAt);

			return Utils.GetLocalTime();
		}

		public Twitter(TwitterContext twitterContext)
		{
		}

		public bool CheckLastTweetNotRecent(Timers timers, out int sleepCounter)
		{
			sleepCounter = 0;
			bool inTimeZone;
			string timePeriod;

			Console.WriteLine("Checking status of last Tweet");
			DateTime lastTweetTime = GetLastTweet();
			DateTime currentTime = Utils.GetLocalTime();
			Console.WriteLine("Last Tweet was at: {0}", lastTweetTime.ToString("dd/MM/yyyy HH:mm:ss"));

			//Check current time isn't out of bounds
			if (timers.CheckOutOfHours(currentTime, out sleepCounter))
				return true;

			else if (lastTweetTime.Date <= currentTime.Date)
			{
				sleepCounter = timers.CheckTimers(lastTweetTime, out timePeriod, out inTimeZone);

				// If the date is the same, lets alignt the timer to the last Tweet time for timing accuracy
				if (lastTweetTime.Date == currentTime.Date && sleepCounter > 0)
				{
					// TODO: Check its within 15mins
					Console.WriteLine("Syncing sleep timer to last Tweet");
					int timeDiff = Math.Abs(Convert.ToInt32(sleepCounter - currentTime.Subtract(lastTweetTime).TotalMilliseconds));
					int overSleepCounter = timers.CheckOverShootTimer(currentTime, sleepCounter);

					if (timeDiff < overSleepCounter)
					{
						Console.WriteLine("Favouring the shorter timer");
						sleepCounter = timeDiff;
					}
					else
					{
						Console.WriteLine("Favouring the adjusted timer");
						sleepCounter = overSleepCounter;
					}
				}

				if (sleepCounter > 0)
				{
					Console.WriteLine("Last Tweet is recent - Sleeping until: {0}", currentTime.AddSeconds(sleepCounter / 1000).ToString("dd/MM/yyyy HH:mm:ss"));
					return true;
				}
				else
				{
					Console.WriteLine("Last Tweet is stale: {0}", lastTweetTime.ToString("dd/MM/yyyy HH:mm:ss"));
					return false;
				}
			}

			return false;
		}
	}
}