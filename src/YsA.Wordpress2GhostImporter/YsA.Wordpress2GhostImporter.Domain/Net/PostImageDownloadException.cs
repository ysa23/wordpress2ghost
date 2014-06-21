using System;

namespace YsA.Wordpress2GhostImporter.Domain.Net
{
	public class PostImageDownloadException : Exception
	{
		public PostImageDownloadException(string postUrl, string imageUrl, Exception inner) 
			: base("Error while trying to download image '" + imageUrl + "' for post '" + postUrl + "'", inner)
		{}
	}
}