using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TwitterTraffic
{
	public static class Utils
	{
		public static T Deserialise<T>(string json)
		{
			JavaScriptSerializer js = new JavaScriptSerializer();
			return js.Deserialize<T>(json);
		}

		public static DateTime GetLocalTime()
		{
			return GetLocalTime(DateTime.UtcNow);
		}

		public static DateTime GetLocalTime(DateTime dateTime)
		{
			return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
		}

		public static int GetUnixNow()
		{
			return (Int32)(GetLocalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		}
	}
}
