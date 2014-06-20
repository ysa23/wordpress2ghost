using System;
using System.IO;
using System.Linq;
using Castle.Windsor;
using Castle.Windsor.Installer;
using YsA.Wordpress2GhostImporter.DataAccess.Json;
using YsA.Wordpress2GhostImporter.DataAccess.Wordpress;
using YsA.Wordpress2GhostImporter.Domain.Ghost;

namespace YsA.Wordpress2GhostImporter
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Please specify that target site (as url) and the target file path (optional - otherwise will be written to console)");
				return;
			}

			var container = new WindsorContainer().Install(FromAssembly.This());

			var crawler = container.Resolve<IWordpressCrawler>();
			var serializer = container.Resolve<IJsonSerializer>();
			var converter = container.Resolve<IGhostConverter>();

			Console.WriteLine("Starting to crawl '{0}'", args[0]);
			var posts = crawler.Crawl(args[0]).ToArray();
			Console.WriteLine("Finished crawling '{0}'. Converting to ghost", args[0]);
			var ghostPosts = converter.FromPosts(posts);
			var serialized = serializer.Serialize(ghostPosts);

			if (args.Length == 1)
			{
				Console.WriteLine(serialized);
				return;
			}

			Console.WriteLine("Finished serializing. Writing to '{0}'", args[1]);
			File.WriteAllText(args[1], serialized);
			Console.WriteLine("Done. All you have to do now is to import to ghost... Enjoy :)");
		}
	}
}
