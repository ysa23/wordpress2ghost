using HtmlAgilityPack;

namespace YsA.Wordpress2GhostImporter.DataAccess.Net
{
	public interface IHtmlReader
	{
		HtmlDocument GetHtmlFromUrl(string targetUrl);
	}

	public class HtmlReader : IHtmlReader
	{
		public HtmlDocument GetHtmlFromUrl(string targetUrl)
		{
			return new HtmlWeb().Load(targetUrl);
		}
	}
}