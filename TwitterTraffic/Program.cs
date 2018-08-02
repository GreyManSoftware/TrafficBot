using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TwitterTraffic
{
	class Program
	{
		static int SleepCounter;
		static bool InTimeZone = false;
		static string TimePeriod;
		private static readonly string TrafficUpdate = "";

		static void Main(string[] args)
		{
#if !DEBUG
			Console.WriteLine("************************************************************");
			// This checks the last update wasn't super recent
			if (TimeChecking.CheckLastTweetNotRecent(out SleepCounter))
			{
				Console.WriteLine("Sleeping until: {0}", Utils.GetLocalTime().AddSeconds(SleepCounter / 1000).ToString("dd/MM/yyyy HH:mm:ss"));
				Thread.Sleep(SleepCounter);
			}
#endif

			while (true)
			{
				Console.WriteLine("************************************************************");
				DateTime currentTime = Utils.GetLocalTime();
				//currentTime = new DateTime(2017, 10, 13, 06, 00, 00);

				// This deals with ensuring that we don't run during quiet housr
				if (TimeChecking.CheckOutOfHours(currentTime, out SleepCounter))
				{
					InTimeZone = false;
					TimePeriod = "OutOfHours";
				}
				else
				{
					// Check if we are in a time zone
					SleepCounter = TimeChecking.CheckTimers(currentTime, out TimePeriod, out InTimeZone);

					// Get latest traffic - We swap the origins and destinations depending on time of day for pessimistic traffic
					int result;
					string origin;
					string destination;

					if (currentTime.Hour < 15)
					{
						origin = "Hengoed";
						destination = "Bedwas";	
					}
					else
					{
						origin = "Bedwas";
						destination = "Hengoed";
					}

					result = TrafficChecks.CheckTrafficDelay(String.Format(TrafficUpdate, origin, destination));

					// Patch up the destination. This is because currently I just want to simplify the route
					destination = destination == "Bedwas" ? "Caerphilly" : "Ystrad";

					Console.WriteLine("Traffic delay: {0}mins", result);

					//Select message to send
					string hashTags = "#Pwllypant #Caerphilly"; // This might want revising
					string message = Messages.SelectMessage(result, out hashTags);
					message = Messages.FormatMessage(message, destination, hashTags, result, currentTime);
					Console.WriteLine("Message to send: {0}", message);

#if !DEBUG
					//Tweet Message
					Task.Run(() => Twitter.PostTweet(message));
#endif

					// Ensure our sleep doesn't over shoot!
					if (!InTimeZone)
						SleepCounter = TimeChecking.CheckOverShootTimer(currentTime, SleepCounter);
				}

				// Finally, take a little nap
				Console.WriteLine("Sleeping until: {0}", currentTime.AddSeconds(SleepCounter / 1000).ToString("dd/MM/yyyy HH:mm:ss"));
				Thread.Sleep(SleepCounter);
			}
		}
	}
}