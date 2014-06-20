using System;

namespace YsA.Wordpress2GhostImporter.Domain.Net
{
	public class PostCrawlingException : Exception
	{
		public PostCrawlingException(string postUrl, Exception ex)
		{}
	}
}