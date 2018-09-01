using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;

namespace TwitterTraffic.Bots
{
	public abstract class Bot
	{
		public abstract string ScreenName { get; }
		protected Twitter TwitterConnection;
		protected TrafficChecks TrafficChecker;
		protected Timers TweetTimer;
		protected Messages Messages;
		protected int SleepCounter;
		protected bool InTimeZone;
		protected string TimePeriod;
		protected DateTime CurrentTime = Utils.GetLocalTime();

		public virtual void Run()
		{
#if !DEBUG
			Console.WriteLine("************************************************************");

			// Start up check to ensure last tweet isn't recent
			if (TwitterConnection.CheckLastTweetNotRecent(TweetTimer, ScreenName, out SleepCounter))
			{
				Console.WriteLine("{0} - Sleeping until: {1}", ScreenName, Utils.GetLocalTime().AddSeconds(SleepCounter / 1000).ToString("dd/MM/yyyy HH:mm:ss"));
				Thread.Sleep(SleepCounter);
			}
#endif
			while (true)
			{
#if DEBUG
				CurrentTime = new DateTime(2018, 08, 31, 09, 00, 00);
#else
				CurrentTime = Utils.GetLocalTime();
#endif
				Console.WriteLine("************************************************************");

				// This deals with ensuring that we don't run during quiet housr
				if (TweetTimer.CheckOutOfHours(CurrentTime, out SleepCounter))
				{
					InTimeZone = false;
					TimePeriod = "OutOfHours";
				}
				else
				{
					// Check if we are in a time zone
					SleepCounter = TweetTimer.CheckTimers(CurrentTime, out TimePeriod, out InTimeZone);

					// Get latest traffic - We swap the origins and destinations depending on time of day for pessimistic traffic
					int result;
					string origin;
					string destination;

					CalculateOriginAndDestination(out origin, out destination);

					result = TrafficChecker.CheckTrafficDelay(origin, destination);

					// Patch up the destination. This is because currently I just want to simplify the route
					//destination = destination == "Bedwas" ? "Caerphilly" : "Ystrad";

					Console.WriteLine("Traffic delay: {0}mins", result);

					//Select message to send
					string hashTags = Messages.HashTags;
					string message = Messages.SelectMessage(result);
					message = Messages.FormatMessage(message, destination, hashTags, result, CurrentTime);
					Console.WriteLine("Message to send: {0}", message);

#if !DEBUG
					//Tweet Message
					Task.Run(() => TwitterConnection.PostTweet(message));
#endif

					// Ensure our sleep doesn't over shoot!
					if (!InTimeZone)
						SleepCounter = TweetTimer.CheckOverShootTimer(CurrentTime, SleepCounter);
				}

				// Finally, take a little nap
				Console.WriteLine("{0} - Sleeping until: {1}", ScreenName, CurrentTime.AddSeconds(SleepCounter / 1000).ToString("dd/MM/yyyy HH:mm:ss"));

#if !DEBUG
				Thread.Sleep(SleepCounter);
#else
				Thread.Sleep(1000);
#endif
			}
		}

		/// <summary>
		/// This method returns an origin and destination. This is virtual as you might wish to change these throughout the day
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="destination"></param>
		public abstract void CalculateOriginAndDestination(out string origin, out string destination);

		protected Bot(TwitterContext twitterContext, string googleConnectionString, Timers tweetTimer, Messages messages)
		{
			TwitterConnection = new Twitter(twitterContext);
			TrafficChecker = new TrafficChecks(googleConnectionString);
			TweetTimer = tweetTimer;
			Messages = messages;
		}
	}
}
