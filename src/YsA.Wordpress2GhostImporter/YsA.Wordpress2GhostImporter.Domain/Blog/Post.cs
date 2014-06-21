using System;
using System.Collections.Generic;

namespace YsA.Wordpress2GhostImporter.Domain.Blog
{
	public class Post
	{
		public string Url { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public Meta Meta { get; set; }
		public DateTime Timestamp { get; set; }
		public IList<Tag> Tags { get; set; }
		public IList<Image> Images { get; set; } 
	}
}