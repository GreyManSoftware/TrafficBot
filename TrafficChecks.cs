using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic
{
	public class TrafficChecks
	{
		private string ConnectionString;

		/// <summary>
		/// Gets latest traffic between origin and destination
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="destination"></param>
		/// <param name="via"></param>
		/// <returns></returns>
		public int CheckTrafficDelay(string origin, string destination, string via = "")
		{
			string jsonTraffic = GetLatestTraffic(String.Format(ConnectionString, origin, destination));
			TrafficReport trafficReport = Utils.Deserialise<TrafficReport>(jsonTraffic);
			int duration = trafficReport.routes[0].legs[0].duration.value;
			int durationInTraffic = trafficReport.routes[0].legs[0].duration_in_traffic.value;
			return Math.Abs((durationInTraffic - duration) / 60);
		}

		private string GetLatestTraffic(string connectionString)
		{
			WebClient wc = new WebClient();
			return wc.DownloadString(connectionString);
		}

		public TrafficChecks(string connectionString)
		{
			ConnectionString = connectionString;
		}
	}
}
