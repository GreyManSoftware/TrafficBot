using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic.Bots.Pwllypant
{
	public class PwllypantMessages : Messages
	{
		public override List<string> ClearMessages => new List<string>
		{
			"Everything is groovy",
			"No traffic here",
			"So lonely, come drive around me!",
			"Free and easy!",
			"Now is your chance!",
			"Go around me a few times whilst you can",
			"Hell hath frozen over, traffic is flowing",
			"Dim traffig",
		};

		public override List<string> LightMessages => new List<string>
		{
			"Light traffic",
			"Not too bad",
			"I've seen worse",
			"Meh",
			"As things go, we're not too bad",
		};

		public override List<string> BusyMessages => new List<string>
		{
			"Pretty busy",
			"One does not simply go around the roundabout",
			"Have you considered walking?",
			"I hope you are sitting comfortably",
		};

		public override List<string> VeryBusyMessages => new List<string>
		{
			"Very busy",
			"Might as well have a swift half down the local",
			"I hope you have enough fuel",
			"You shall not pass",
			"Today's delay brought to you by #CCBC",
			"How can one roundabout be so fucked?",
		};

		public override List<string> ExtremelyBusyMessages => new List<string>
		{
			"Stupid busy",
			"Might as well get out and crawl",
			"Abandon hope all ye who enter here",
			"It would be quicker to build a raft and sail the Rhymney River",
			"I'm no traffic scientist, but this is fucked",
			"Where ever you going is almost certainly not worth it",
			"By the time you get where you're going, pigs will be flying",
			"As roundabouts go, I'm pretty fucked",
			"It's busy, so no fighting!",
		};

		public override List<string> CrazyBusyMessages => new List<string>
		{
			"Mental",
			"Go home",
			"I hope you packed a tent",
			"Give up...",
		};

		public override List<string> UnpassableMessages => new List<string>
		{
			"The roads are totally and utterly fucked, don't even look at your car!",
			"Go home, it's not worth it",
			"It's like a scene from #TheWalkingDead",
			"It might be time to consider moving to another country"
		};

		public override string HashTags => "#Pwllypant #Caerphilly";
	}
}
