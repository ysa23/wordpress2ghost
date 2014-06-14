namespace YsA.Wordpress2GhostImporter.Domain.Ghost
{
	public class ImportData
	{
		public Meta Meta { get; set; }
		public Post[] Posts { get; set; }
		public Tag[] Tags { get; set; }
		public PostTag[] PostTags { get; set; }
	}
}