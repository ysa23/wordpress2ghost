using System;

namespace YsA.Wordpress2GhostImporter.Domain.Ghost
{
	public class Tag
	{
		public long Id { get; set; }
		public string Uuid { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Description { get; set; }
		public long? ParentId { get; set; }
		public string MetaTitle { get; set; }
		public string MetaDescription { get; set; }
		public DateTime CreatedAt { get; set; }
		public long CreatedBy { get; set; }
		public DateTime UpdatedAt { get; set; }
		public long UpdatedBy { get; set; }
	}
}