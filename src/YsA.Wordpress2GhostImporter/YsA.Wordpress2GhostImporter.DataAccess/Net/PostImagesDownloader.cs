using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YsA.Wordpress2GhostImporter.Domain.Blog;
using YsA.Wordpress2GhostImporter.Domain.Net;

namespace YsA.Wordpress2GhostImporter.DataAccess.Net
{
	public class PostImagesDownloader : IPostImagesDownloader
	{
		private readonly IImageDownloader _imageDownloader;

		public PostImagesDownloader(IImageDownloader imageDownloader)
		{
			_imageDownloader = imageDownloader;
		}

		public void DownloadImages(IList<Post> posts, string targetFolder)
		{
			Directory.CreateDirectory(targetFolder);

			foreach(var post in posts.Where(x => x.Images != null && x.Images.Count > 0))
			{
				var postDirectoryPath = Path.Combine(targetFolder, RemoveInvalidCharacters(post.Title));
				Directory.CreateDirectory(postDirectoryPath);

				foreach (var image in post.Images)
				{
					_imageDownloader.DownloadImage(image.Url, Path.Combine(postDirectoryPath, image.Name));
				}
			}
		}

		private static string RemoveInvalidCharacters(string name)
		{
			var invalidPathCharacters = string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(new string(Path.GetInvalidFileNameChars())));

			return Regex.Replace(name, invalidPathCharacters, "_");
		}
	}
}