using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic
{
	public static class TrafficChecks
	{
		public static int CheckTrafficDelay(string connectionString)
		{
			string jsonTraffic = GetLatestTraffic(connectionString);
			TrafficReport trafficReport = Utils.Deserialise<TrafficReport>(jsonTraffic);
			int duration = trafficReport.routes[0].legs[0].duration.value;
			int durationInTraffic = trafficReport.routes[0].legs[0].duration_in_traffic.value;
			return Math.Abs((durationInTraffic - duration) / 60);
		}

		public static string GetLatestTraffic(string connectionString)
		{
			WebClient wc = new WebClient();
			return wc.DownloadString(connectionString);
		}
	}
}
