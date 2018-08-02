using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterTraffic
{
	public static class TimeChecking
	{
		/// <summary>
		/// Checks to see if the given time stamp lands within a traffic time region
		/// </summary>
		public static int CheckTimers(DateTime currentTime, out string period, out bool inTimeZone)
		{
			int sleepCounter;

			if (currentTime.IsWeekend())
			{
				period = "Weekend";
				Console.WriteLine("{0} period - {1}mins per Tweet", period, 120);
				inTimeZone = true;
				return 120 * 60 * 1000;
			}
			else
			{
				foreach (string timePeriodKey in Timers.TimerDictionary.Keys.Where(k => k != "WeekdayOutOfHours" && k != "WeekendOutOfHours"))
				{
					if (currentTime.CompareTo(Timers.TimerDictionary[timePeriodKey][0]) >= 0 && currentTime.CompareTo(Timers.TimerDictionary[timePeriodKey][1]) <= 0)
					{
						period = timePeriodKey;
						inTimeZone = true;
						int sleepTime = Timers.TimerSleep[timePeriodKey] * 60 * 1000;
						Console.WriteLine("{0} period - {1}mins per Tweet", period, Timers.TimerSleep[timePeriodKey]);
						return sleepTime;
					}
				}
			}

			sleepCounter = 60 * 60 * 1000;
			period = "none";
			inTimeZone = false;

			return sleepCounter;
		}

		/// <summary>
		/// Checks to see if the timer is during an out of hours period. If true, it will return a timer to get to the start time
		/// </summary>
		public static bool CheckOutOfHours(DateTime currentTime, out int sleepCounter)
		{
			if (currentTime.IsWeekend())
			{
				if (CheckWeekendOutOfHours(currentTime, out sleepCounter))
				{
					Console.WriteLine("Weekend out of hours found");
					if (currentTime.AddMilliseconds(sleepCounter).DayOfWeek == DayOfWeek.Monday)
					{
						Console.WriteLine("New timer breaches into weekday, adjusting");
						sleepCounter = sleepCounter - Math.Abs(Convert.ToInt32((Timers.TimerDictionary["WeekdayOutOfHours"][0] - Timers.TimerDictionary["WeekendOutOfHours"][0]).TotalMilliseconds));
					}
					return true;
				}
			}
			else
			{
				if (CheckWeekdayOutOfHours(currentTime, out sleepCounter))
				{
					Console.WriteLine("Weekday out of hours found");
					if (currentTime.AddMilliseconds(sleepCounter).DayOfWeek == DayOfWeek.Saturday)
					{
						Console.WriteLine("New timer breaches into weekend, adjusting");
						sleepCounter = sleepCounter + Math.Abs(Convert.ToInt32((Timers.TimerDictionary["WeekdayOutOfHours"][0] - Timers.TimerDictionary["WeekendOutOfHours"][0]).TotalMilliseconds));
					}
					return true;
				}
			}

			sleepCounter = 0;
			return false;
		}

		public static bool CheckWeekdayOutOfHours(DateTime currentTime, out int sleepCounter)
		{
			if (currentTime.CompareTo(Timers.TimerDictionary["WeekdayOutOfHours"][0]) < 0 && currentTime.Date == Timers.TimerDictionary["WeekdayOutOfHours"][0].Date)
			{
				Console.WriteLine("Early morning silent hours");
				sleepCounter = Math.Abs(Convert.ToInt32(currentTime.Subtract(Timers.TimerDictionary["WeekdayOutOfHours"][0]).TotalSeconds) * 1000);
				return true;
			}
			// If this statement is true, then we will need to re-generate the timers because the comparisons will be wrong
			else if (currentTime.CompareTo(Timers.TimerDictionary["WeekdayOutOfHours"][1]) > 0)
			{
				Timers.GenerateTimers(1);
				Console.WriteLine("Late night silent hours");
				sleepCounter = Math.Abs(Convert.ToInt32(currentTime.Subtract(Timers.TimerDictionary["WeekdayOutOfHours"][0]).TotalSeconds)) * 1000;

				return true;
			}

			sleepCounter = 0;
			return false;
		}

		public static bool CheckWeekendOutOfHours(DateTime currentTime, out int sleepCounter)
		{
			if (currentTime.CompareTo(Timers.TimerDictionary["WeekendOutOfHours"][0]) < 0 && currentTime.Date == Timers.TimerDictionary["WeekendOutOfHours"][0].Date)
			{
				Console.WriteLine("Early morning weekend silent hours");
				sleepCounter = Math.Abs(Convert.ToInt32(currentTime.Subtract(Timers.TimerDictionary["WeekendOutOfHours"][0]).TotalSeconds) * 1000);
				return true;
			}
			// If this statement is true, then we will need to re-generate the timers because the comparisons will be wrong
			else if (currentTime.CompareTo(Timers.TimerDictionary["WeekendOutOfHours"][1]) > 0)
			{
				Timers.GenerateTimers(1);
				Console.WriteLine("Late night weekend silent hours");
				sleepCounter = Math.Abs(Convert.ToInt32(currentTime.Subtract(Timers.TimerDictionary["WeekendOutOfHours"][0]).TotalSeconds)) * 1000;
				return true;
			}

			sleepCounter = 0;
			return false;
		}

		public static int CheckOverShootTimer(DateTime currentTime, int sleepCounter)
		{
			DateTime futureTime = currentTime.AddSeconds(sleepCounter / 1000);

			foreach (string timePeriodKey in Timers.TimerDictionary.Keys.Where(k => k != "WeekdayOutOfHours" && k != "WeekendOutOfHours"))
			{
				if (futureTime.CompareTo(Timers.TimerDictionary[timePeriodKey][0]) >= 0 && futureTime.CompareTo(Timers.TimerDictionary[timePeriodKey][1]) <= 0)
				{
					Console.WriteLine("Adjusting timer to not miss {0} timer", timePeriodKey);
					return Math.Abs(sleepCounter - Math.Abs(Convert.ToInt32(futureTime.Subtract(Timers.TimerDictionary[timePeriodKey][0]).TotalMilliseconds)));
				}
			}

			return sleepCounter;
		}

		public static bool CheckLastTweetNotRecent(out int sleepCounter)
		{
			sleepCounter = 0;
			bool inTimeZone;
			string timePeriod;

			Console.WriteLine("Checking status of last Tweet");
			DateTime lastTweetTime = Twitter.GetLastTweet();
			DateTime currentTime = Utils.GetLocalTime();
			Console.WriteLine("Last Tweet was at: {0}", lastTweetTime.ToString("dd/MM/yyyy HH:mm:ss"));

			//Check current time isn't out of bounds
			if (TimeChecking.CheckOutOfHours(currentTime, out sleepCounter))
				return true;

			else if (lastTweetTime.Date <= currentTime.Date)
			{
				sleepCounter = TimeChecking.CheckTimers(lastTweetTime, out timePeriod, out inTimeZone);

				// If the date is the same, lets alignt the timer to the last Tweet time for timing accuracy
				if (lastTweetTime.Date == currentTime.Date && sleepCounter > 0)
				{
					// TODO: Check its within 15mins
					Console.WriteLine("Syncing sleep timer to last Tweet");
					int timeDiff = Math.Abs(Convert.ToInt32(sleepCounter - currentTime.Subtract(lastTweetTime).TotalMilliseconds));
					int overSleepCounter = TimeChecking.CheckOverShootTimer(currentTime, sleepCounter);

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
