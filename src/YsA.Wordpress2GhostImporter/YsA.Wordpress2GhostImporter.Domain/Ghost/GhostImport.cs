namespace YsA.Wordpress2GhostImporter.Domain.Ghost
{
	public class GhostImport
	{
		public GhostImport(GhostImportData data)
		{
			Meta = new GhostMeta();
			Data = data;
		}

		public GhostMeta Meta { get; set; }
		public GhostImportData Data { get; set; }
	}

	public class GhostImportData
	{
		public GhostPost[] Posts { get; set; }
		public GhostTag[] Tags { get; set; }
		public PostTag[] PostsTags { get; set; }
	}
}