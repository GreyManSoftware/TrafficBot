using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterTraffic.Bots;
using TwitterTraffic.Bots.BootsCorner;
using TwitterTraffic.Bots.Pwllypant;

namespace TwitterTraffic
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Starting Pwllypant Task");
			Task pwllypantTask = Task.Run(() =>
			{
				Pwllypant pwllypant = new Pwllypant();
				pwllypant.Run();

			});

			Console.WriteLine("Starting Boots Corner Task");
			Task bootscornerTask = Task.Run(() =>
			{
				BootsCorner bootsCorner = new BootsCorner();
				bootsCorner.Run();
			});

			pwllypantTask.Wait();
			bootscornerTask.Wait();
		}
	}
}