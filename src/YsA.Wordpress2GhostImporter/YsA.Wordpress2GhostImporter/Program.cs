using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
using YsA.Wordpress2GhostImporter.Domain.Wordpress;

namespace YsA.Wordpress2GhostImporter
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Please specify that target site (as url) and the target folder path (optional - otherwise will be written to console), and import file name (default will be import.json)");
				return;
			}

			var container = new WindsorContainer().Install(FromAssembly.This());

			var exporter = container.Resolve<IWordpressExporter>();

			exporter.ExportToGhost(args[0], args.Length > 1 ? args[1] : string.Empty, args.Length > 2 ? args[2] : "import.json");
		}
	}
}
