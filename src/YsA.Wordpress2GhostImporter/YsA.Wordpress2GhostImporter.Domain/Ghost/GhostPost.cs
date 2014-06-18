using System;

namespace YsA.Wordpress2GhostImporter.Domain.Ghost
{
	public class GhostPost
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string Markdown { get; set; }
		public string Html { get; set; }
		public string Status { get { return "published"; } }
		public string MetaTitle { get; set; }
		public string MetaDescription { get; set; }
		public long AuthorId { get; set; }
		public DateTime CreatedAt { get; set; }
		public long CreatedBy { get; set; }
		public DateTime PublishedAt { get; set; }
		public long PublishedBy { get; set; }
	}
}
