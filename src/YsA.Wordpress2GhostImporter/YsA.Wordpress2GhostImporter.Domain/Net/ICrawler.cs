namespace YsA.Wordpress2GhostImporter.Domain.Net
{
	public interface ICrawler<T>
	{
		T Crawl(string targetUrl);
	}
}