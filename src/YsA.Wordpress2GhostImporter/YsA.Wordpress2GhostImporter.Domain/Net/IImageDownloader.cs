namespace YsA.Wordpress2GhostImporter.Domain.Net
{
	public interface IImageDownloader
	{
		void DownloadImage(string url, string target);
	}
}