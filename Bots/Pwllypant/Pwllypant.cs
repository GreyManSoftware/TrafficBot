using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace TwitterTraffic.Bots.Pwllypant
{
	public class Pwllypant : Bot
	{
		public override string ScreenName => "PwllypantRound";
		private static SingleUserAuthorizer TwitterAuthToken = new SingleUserAuthorizer
		{
			CredentialStore = new SingleUserInMemoryCredentialStore
			{
			}
		};

		private static string GoogleConnectionString = @"https://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&waypoints=via:A469,A468&departure_time=now&key=AIzaSyBZZgh98WMp7QnSGwmmp1tx7Ju_b8qhlrU";

		public override void CalculateOriginAndDestination(out string origin, out string destination)
		{
			if (CurrentTime.Hour < 15)
			{
				origin = "Hengoed";
				destination = "Bedwas";
			}
			else
			{
				origin = "Bedwas";
				destination = "Hengoed";
			}
		}

		public Pwllypant() : base(new TwitterContext(TwitterAuthToken), GoogleConnectionString, new PwllypantTimers(), new PwllypantMessages())
		{
		}
	}
}
