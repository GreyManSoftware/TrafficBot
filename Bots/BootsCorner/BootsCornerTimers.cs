using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic.Bots.BootsCorner
{
	public class BootsCornerTimers : Timers
	{
		private static Dictionary<string, List<DateTime>> Timer = new Dictionary<string, List<DateTime>>
		{
			{ "EarlyMorning", new List<DateTime> { Convert.ToDateTime("06:15:00"), Convert.ToDateTime("07:30:00") } },
			{ "Morning", new List<DateTime> { Convert.ToDateTime("07:45:00"), Convert.ToDateTime("09:50:00") } },
			{ "Lunch", new List<DateTime> { Convert.ToDateTime("11:45:00"), Convert.ToDateTime("12:50:00") } },
			{ "Home", new List<DateTime> {  Convert.ToDateTime("15:00:00"), Convert.ToDateTime("18:00:00") } },
			{ "WeekdayOutOfHours", new List<DateTime> { Convert.ToDateTime("06:00:00"), Convert.ToDateTime("21:30:00") } },
			{ "WeekendOutOfHours", new List<DateTime> { Convert.ToDateTime("10:00:00"), Convert.ToDateTime("20:00:00") } },
		};

		public BootsCornerTimers() : base(Timer)
		{
		}
	}
}