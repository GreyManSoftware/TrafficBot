using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTraffic.Bots.BootsCorner
{
	public class BootsCornerMessages : Messages
	{
		public override List<string> ClearMessages => new List<string>
		{
			"Everything is groovy",
			"No traffic here",
			"Free and easy!",
			"Now is your chance!",
			"Hell hath frozen over, traffic is flowing"
		};

		public override List<string> LightMessages => new List<string>
		{
			"Light traffic",
			"Not too bad",
			"I've seen worse",
			"Meh",
		};

		public override List<string> BusyMessages => new List<string>
		{
			"Pretty busy",
			"Have you considered walking?",
			"I hope you are sitting comfortably",
			"It has gone to shit",
			"This delay is brought to you by Cheltenham Borough Council",
		};

		public override List<string> VeryBusyMessages => new List<string>
		{
			"Very busy",
			"Might as well have a swift half down the local",
			"I hope you have enough fuel",
			"You shall not pass",
			"This \"trial\" is working like a champ",
		};

		public override List<string> ExtremelyBusyMessages => new List<string>
		{
			"Stupid busy",
			"Might as well get out and crawl",
			"Abandon hope all ye who enter here",
			"I'm no traffic scientist, but this is fucked",
			"Where ever you going is almost certainly not worth it",
			"By the time you get where you're going, pigs will be flying",
			"It's busy, so no fighting!",
			"Don't forget to official log your displeasure with CBC",
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

		public override string HashTags => "#BootsCorner #Cheltenham #CBC #TrafficBot";
	}
}
