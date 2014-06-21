using System.Net;
using YsA.Wordpress2GhostImporter.Domain.Net;

namespace YsA.Wordpress2GhostImporter.DataAccess.Net
{
	public class ImageDownloader : IImageDownloader
	{
		public void DownloadImage(string url, string target)
		{
			using (var webClient = new WebClient())
			{
				webClient.DownloadFile(url, target);
			}
		}
	}
}