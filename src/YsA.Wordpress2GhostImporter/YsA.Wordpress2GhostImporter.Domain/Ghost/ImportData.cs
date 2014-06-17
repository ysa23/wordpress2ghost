namespace YsA.Wordpress2GhostImporter.Domain.Ghost
{
	public class ImportData
	{
		public GhostMeta Meta { get; set; }
		public GhostPost[] Posts { get; set; }
		public GhostTag[] Tags { get; set; }
		public PostTag[] PostTags { get; set; }
	}
}