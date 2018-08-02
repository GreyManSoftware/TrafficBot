using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic
{
	public static class Twitter
	{
		private static TwitterContext TwitterConnection = ConnectToTwitter();

		public static TwitterContext ConnectToTwitter()
		{
			SingleUserAuthorizer auth = new SingleUserAuthorizer
			{
				CredentialStore = new SingleUserInMemoryCredentialStore
				{
				}
			};

			return new TwitterContext(auth);
		}

		public static async void PostTweet(string message)
		{
			try
			{
				await TwitterConnection.TweetAsync(message);
				Console.WriteLine("Updated status successfully");
			}
			catch
			{
				Console.WriteLine("Status hasn't changed");
			}
		}

		public static DateTime GetLastTweet()
		{
			Status lastTweet = TwitterConnection.Status.Where(s => s.Type == StatusType.User && s.ScreenName == "PwllypantRound" && s.InReplyToScreenName == null).FirstOrDefault();
			//Console.WriteLine(lastTweet.Text);
			if (lastTweet != null)
				return Utils.GetLocalTime(lastTweet.CreatedAt);

			return Utils.GetLocalTime();
		}
	}
}
