using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace TwitterTraffic.Bots.BootsCorner
{
	public class BootsCorner : Bot
	{
		private static SingleUserAuthorizer TwitterAuthToken = new SingleUserAuthorizer
		{
			CredentialStore = new SingleUserInMemoryCredentialStore
			{
			}
		};

		private static string GoogleConnectionString = @"https://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&waypoints=via:A46,A435&departure_time=now&key=AIzaSyBZZgh98WMp7QnSGwmmp1tx7Ju_b8qhlrU";

		public override void CalculateOriginAndDestination(out string origin, out string destination)
		{
			origin = "GL503JR";
			destination = "Cheltenham Race Course";
		}

		public BootsCorner() : base(new TwitterContext(TwitterAuthToken), GoogleConnectionString, new BootsCornerTimers(), new BootsCornerMessages())
		{
		}
	}
}
