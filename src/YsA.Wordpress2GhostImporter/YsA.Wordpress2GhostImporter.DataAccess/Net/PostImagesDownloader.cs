using System;
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
					try
					{
						_imageDownloader.DownloadImage(GetImageUrl(post, image), Path.Combine(postDirectoryPath, image.Name));
					}
					catch (Exception ex)
					{
						throw new PostImageDownloadException(post.Url, image.Url, ex);
					}
				}
			}
		}

		public string GetImageUrl(Post post, Image image)
		{
			var imageUri = new Uri(image.Url, UriKind.RelativeOrAbsolute);
			if (imageUri.IsAbsoluteUri)
				return imageUri.AbsoluteUri;

			var postUri = new Uri(post.Url, UriKind.Absolute);

			return postUri.Scheme + "://" + postUri.Authority + image.Url;
		}

		private static string RemoveInvalidCharacters(string name)
		{
			var invalidPathCharacters = string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(new string(Path.GetInvalidFileNameChars())));

			return Regex.Replace(name, invalidPathCharacters, "_");
		}
	}
}