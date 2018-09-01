using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic.Bots
{
	public abstract class Timers
	{
		public Dictionary<string, List<DateTime>> TimerDictionary { get; set; }

		public virtual Dictionary<string, int> TimerSleep => new Dictionary<string, int>
		{
			{ "EarlyMorning", 15 },
			{ "Morning", 15 },
			{ "Lunch", 20 },
			{ "Home", 15 },
		};

		/// <summary>
		/// This generates the timers. Call this each day so that the time comparisons are for the right day.
		/// </summary>
		public void GenerateTimers(int addDays)
		{
			TimerDictionary["EarlyMorning"][0] = TimerDictionary["EarlyMorning"][0].AddDays(addDays);
			TimerDictionary["EarlyMorning"][1] = TimerDictionary["EarlyMorning"][1].AddDays(addDays);
			TimerDictionary["Morning"][0] = TimerDictionary["Morning"][0].AddDays(addDays);
			TimerDictionary["Morning"][1] = TimerDictionary["Morning"][1].AddDays(addDays);
			TimerDictionary["Lunch"][0] = TimerDictionary["Lunch"][0].AddDays(addDays);
			TimerDictionary["Lunch"][1] = TimerDictionary["Lunch"][1].AddDays(addDays);
			TimerDictionary["Home"][0] = TimerDictionary["Home"][0].AddDays(addDays);
			TimerDictionary["Home"][1] = TimerDictionary["Home"][1].AddDays(addDays);
			TimerDictionary["WeekdayOutOfHours"][0] = TimerDictionary["WeekdayOutOfHours"][0].AddDays(addDays);
			TimerDictionary["WeekdayOutOfHours"][1] = TimerDictionary["WeekdayOutOfHours"][1].AddDays(addDays);
			TimerDictionary["WeekendOutOfHours"][0] = TimerDictionary["WeekendOutOfHours"][0].AddDays(addDays);
			TimerDictionary["WeekendOutOfHours"][1] = TimerDictionary["WeekendOutOfHours"][1].AddDays(addDays);
		}

		/// <summary>
		/// Checks to see if the given time stamp lands within a traffic time region
		/// </summary>
		public int CheckTimers(DateTime currentTime, out string period, out bool inTimeZone)
		{
			int sleepCounter;

			if (currentTime.IsWeekend())
			{
				period = "Weekend";
				Console.WriteLine("{0} period - {1}mins per Tweet", period, 120);
				inTimeZone = true;
				return 120 * 60 * 1000;
			}

			foreach (string timePeriodKey in TimerDictionary.Keys.Where(k => k != "WeekdayOutOfHours" && k != "WeekendOutOfHours"))
			{
				if (currentTime.CompareTo(TimerDictionary[timePeriodKey][0]) >= 0 &&
				    currentTime.CompareTo(TimerDictionary[timePeriodKey][1]) <= 0)
				{
					period = timePeriodKey;
					inTimeZone = true;
					int sleepTime = TimerSleep[timePeriodKey] * 60 * 1000;
					Console.WriteLine("{0} period - {1}mins per Tweet", period, TimerSleep[timePeriodKey]);
					return sleepTime;
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
		public  bool CheckOutOfHours(DateTime currentTime, out int sleepCounter)
		{
			if (currentTime.IsWeekend())
			{
				if (CheckWeekendOutOfHours(currentTime, out sleepCounter))
				{
					Console.WriteLine("Weekend out of hours found");
					if (currentTime.AddMilliseconds(sleepCounter).DayOfWeek == DayOfWeek.Monday)
					{
						Console.WriteLine("New timer breaches into weekday, adjusting");
						sleepCounter = sleepCounter - Math.Abs(Convert.ToInt32((TimerDictionary["WeekdayOutOfHours"][0] - TimerDictionary["WeekendOutOfHours"][0]).TotalMilliseconds));
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
						sleepCounter = sleepCounter + Math.Abs(Convert.ToInt32((TimerDictionary["WeekdayOutOfHours"][0] - TimerDictionary["WeekendOutOfHours"][0]).TotalMilliseconds));
					}
					return true;
				}
			}

			sleepCounter = 0;
			return false;
		}

		public bool CheckWeekdayOutOfHours(DateTime currentTime, out int sleepCounter)
		{
			if (currentTime.CompareTo(TimerDictionary["WeekdayOutOfHours"][0]) < 0 && currentTime.Date == TimerDictionary["WeekdayOutOfHours"][0].Date)
			{
				Console.WriteLine("Early morning silent hours");
				sleepCounter = Math.Abs(Convert.ToInt32(currentTime.Subtract(TimerDictionary["WeekdayOutOfHours"][0]).TotalSeconds) * 1000);
				return true;
			}
			// If this statement is true, then we will need to re-generate the timers because the comparisons will be wrong
			else if (currentTime.CompareTo(TimerDictionary["WeekdayOutOfHours"][1]) > 0)
			{
				GenerateTimers(1);
				Console.WriteLine("Late night silent hours");
				sleepCounter = Math.Abs(Convert.ToInt32(currentTime.Subtract(TimerDictionary["WeekdayOutOfHours"][0]).TotalSeconds)) * 1000;

				return true;
			}

			sleepCounter = 0;
			return false;
		}

		public bool CheckWeekendOutOfHours(DateTime currentTime, out int sleepCounter)
		{
			if (currentTime.CompareTo(TimerDictionary["WeekendOutOfHours"][0]) < 0 && currentTime.Date == TimerDictionary["WeekendOutOfHours"][0].Date)
			{
				Console.WriteLine("Early morning weekend silent hours");
				sleepCounter = Math.Abs(Convert.ToInt32(currentTime.Subtract(TimerDictionary["WeekendOutOfHours"][0]).TotalSeconds) * 1000);
				return true;
			}
			// If this statement is true, then we will need to re-generate the timers because the comparisons will be wrong
			else if (currentTime.CompareTo(TimerDictionary["WeekendOutOfHours"][1]) > 0)
			{
				GenerateTimers(1);
				Console.WriteLine("Late night weekend silent hours");
				sleepCounter = Math.Abs(Convert.ToInt32(currentTime.Subtract(TimerDictionary["WeekendOutOfHours"][0]).TotalSeconds)) * 1000;
				return true;
			}

			sleepCounter = 0;
			return false;
		}

		public int CheckOverShootTimer(DateTime currentTime, int sleepCounter)
		{
			DateTime futureTime = currentTime.AddSeconds(sleepCounter / 1000);

			foreach (string timePeriodKey in TimerDictionary.Keys.Where(k => k != "WeekdayOutOfHours" && k != "WeekendOutOfHours"))
			{
				if (futureTime.CompareTo(TimerDictionary[timePeriodKey][0]) >= 0 && futureTime.CompareTo(TimerDictionary[timePeriodKey][1]) <= 0)
				{
					Console.WriteLine("Adjusting timer to not miss {0} timer", timePeriodKey);
					return Math.Abs(sleepCounter - Math.Abs(Convert.ToInt32(futureTime.Subtract(TimerDictionary[timePeriodKey][0]).TotalMilliseconds)));
				}
			}

			return sleepCounter;
		}

		public Timers(Dictionary<string, List<DateTime>> timer)
		{
			TimerDictionary = timer;
		}
	}
}