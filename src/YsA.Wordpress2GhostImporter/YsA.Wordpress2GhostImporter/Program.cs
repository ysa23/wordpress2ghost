using System;
using System.Linq;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Newtonsoft.Json;
using YsA.Wordpress2GhostImporter.DataAccess.Json;
using YsA.Wordpress2GhostImporter.DataAccess.Wordpress;

namespace YsA.Wordpress2GhostImporter
{
	class Program
	{
		static void Main()
		{
			var container = new WindsorContainer().Install(FromAssembly.This());

			var crawler = container.Resolve<IWordpressCrawler>();
			var serializer = container.Resolve<IJsonSerializer>();

			var posts = crawler.Crawl("http://blogs.microsoft.co.il/ysa").First();
			Console.WriteLine(serializer.Serialize(posts));
			Console.WriteLine(JsonConvert.SerializeObject(posts, new JsonSerializerSettings { Formatting = Formatting.Indented }));
		}
	}
}
