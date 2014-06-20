using System;

namespace YsA.Wordpress2GhostImporter.Domain.Blog
{
	public class PostIsNotTaggedException : Exception
	{
		public PostIsNotTaggedException() : base("Post is not tagged by any tag")
		{}
	}
}