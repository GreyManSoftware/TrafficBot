using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic
{
	public static class Timers
	{
		public static Dictionary<string, List<DateTime>> TimerDictionary = new Dictionary<string, List<DateTime>>
		{
			{ "EarlyMorning", new List<DateTime> { Convert.ToDateTime("06:15:00"), Convert.ToDateTime("07:30:00") } },
			{ "Morning", new List<DateTime> { Convert.ToDateTime("07:45:00"), Convert.ToDateTime("09:50:00") } },
			{ "Lunch", new List<DateTime> { Convert.ToDateTime("11:45:00"), Convert.ToDateTime("12:50:00") } },
			{ "Home", new List<DateTime> {  Convert.ToDateTime("15:00:00"), Convert.ToDateTime("18:00:00") } },
			{ "WeekdayOutOfHours", new List<DateTime> { Convert.ToDateTime("06:00:00"), Convert.ToDateTime("21:30:00") } },
			{ "WeekendOutOfHours", new List<DateTime> { Convert.ToDateTime("10:00:00"), Convert.ToDateTime("20:00:00") } },
		};

		public static Dictionary<string, int> TimerSleep = new Dictionary<string, int>
		{
			{ "EarlyMorning", 15 },
			{ "Morning", 15 },
			{ "Lunch", 20 },
			{ "Home", 15 },
		};

		/// <summary>
		/// This generates the timers. Call this each day so that the time comparisons are for the right day.
		/// </summary>
		public static void GenerateTimers(int addDays)
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
	}
}