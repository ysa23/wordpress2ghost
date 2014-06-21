namespace YsA.Wordpress2GhostImporter.Domain.Blog
{
	public class Image
	{
		public Image(string url)
		{
			Url = url;
			Name = url.Substring(url.LastIndexOf('/') + 1);
		}

		public string Url { get; set; }
		public string Name { get; set; }
	}
}