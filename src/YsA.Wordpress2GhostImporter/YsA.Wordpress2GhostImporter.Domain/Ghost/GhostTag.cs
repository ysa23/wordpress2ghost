using System;

namespace YsA.Wordpress2GhostImporter.Domain.Ghost
{
	public class GhostTag
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedAt { get; set; }
		public long CreatedBy { get; set; }
	}
}