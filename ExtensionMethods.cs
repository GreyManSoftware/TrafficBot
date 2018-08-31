using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic
{
	public static class ExtensionMethods
	{
		public static bool IsWeekend (this DateTime dateTime)
		{
			if (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
				return true;

			return false;
		}
	}
}
