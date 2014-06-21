using System.IO;
using System.Linq;
using YsA.Wordpress2GhostImporter.Domain.Ghost;
using YsA.Wordpress2GhostImporter.Domain.Net;
using YsA.Wordpress2GhostImporter.Domain.Writers;

namespace YsA.Wordpress2GhostImporter.Domain.Wordpress
{
	public interface IWordpressExporter
	{
		void ExportToGhost(string targetUrl, string targetFolder, string targetGhostImportDataFile = "import.json");
	}

	public class WordpressExporter : IWordpressExporter
	{
		private readonly IWordpressCrawler _crawler;
		private readonly IGhostConverter _ghostConverter;
		private readonly IPostImagesDownloader _imagesDownloader;
		private readonly IJsonSerializer _serializer;
		private readonly IOutputWriter _output;

		public WordpressExporter(IWordpressCrawler crawler,
			IGhostConverter ghostConverter,
			IPostImagesDownloader imagesDownloader,
			IJsonSerializer serializer,
			IOutputWriter output)
		{
			_crawler = crawler;
			_ghostConverter = ghostConverter;
			_imagesDownloader = imagesDownloader;
			_serializer = serializer;
			_output = output;
		}

		public void ExportToGhost(string targetUrl, string targetFolder, string targetGhostImportDataFile = "import.json")
		{
			_output.WriteLine("Starting to crawl '{0}'", targetUrl);
			var posts = _crawler.Crawl(targetUrl).ToArray();
			_output.WriteLine("Finished crawling '{0}'. Converting to ghost", targetUrl);
			var ghostPosts = _ghostConverter.FromPosts(posts);
			var serialized = _serializer.Serialize(ghostPosts);

			if (string.IsNullOrEmpty(targetFolder))
			{
				_output.WriteLine(serialized);
				return;
			}

			_output.WriteLine("Creating target folder '{0}' (all existing contents will be deleted)", targetFolder);
			if (Directory.Exists(targetFolder))
				Directory.Delete(targetFolder, true);
			Directory.CreateDirectory(targetFolder);

			var ghostImportedDataFileFullPath = Path.Combine(targetFolder, targetGhostImportDataFile);
			_output.WriteLine("Writing ghost imported data to '{0}'", ghostImportedDataFileFullPath);
			File.WriteAllText(ghostImportedDataFileFullPath, serialized);

			_output.WriteLine("Downloading images from posts to '{0}'", targetFolder);
			_imagesDownloader.DownloadImages(posts, targetFolder);
			
			_output.WriteLine("Done. All you have to do now is to import to ghost... Enjoy :)");
		}
	}
}