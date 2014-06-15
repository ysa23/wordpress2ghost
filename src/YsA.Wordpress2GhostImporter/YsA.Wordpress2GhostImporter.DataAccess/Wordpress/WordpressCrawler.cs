using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using YsA.Wordpress2GhostImporter.Domain.Blog;
using YsA.Wordpress2GhostImporter.Domain.Net;

namespace YsA.Wordpress2GhostImporter.DataAccess.Wordpress
{
	public class WordpressCrawler : ICrawler<IEnumerable<Post>>
	{
		public IEnumerable<Post> Crawl(string targetUrl)
		{
			var web = new HtmlWeb();
			var html = web.Load(targetUrl);

			while (true)
			{
				foreach (var post in html.DocumentNode
					.SelectNodes("//article/h2/a")
					.Select(x => CrawlPost(x.Attributes["href"].Value)))
				{
					yield return post;
				}

				var nextPageLink = html.DocumentNode.SelectSingleNode("//section/div[@class='navigation clearfix']").SelectSingleNode("descendant::a[last()]");
				if (nextPageLink == null || !nextPageLink.InnerText.Contains("next"))
					yield break;

				html = web.Load(nextPageLink.Attributes["href"].Value);
			}
		}

		private Post CrawlPost(string postUrl)
		{
			var web = new HtmlWeb();
			var html = web.Load(postUrl);

			var content = html.DocumentNode.SelectSingleNode("//section/article/div[@class='contx entry-content clearfix']");

			RemoveRedundentContent(content);

			return new Post
			{
				Url = postUrl,
				Title = html.DocumentNode.SelectSingleNode("//section/article/h2/a").InnerText,
				Content =
					content.InnerHtml,
				Timestamp = GetTimestamp(html),
				Meta = GetMeta(html),
				Tags = GetTags(html)
			};
		}

		private void RemoveRedundentContent(HtmlNode content)
		{
			var facebookOptions = content
				.Descendants("iframe").First(x => x.Attributes["src"].Value.StartsWith("//www.facebook.com"));

			content.RemoveChild(facebookOptions);

			var entryOptions = content.SelectSingleNode("descendant::div[@class='clearfix entry-options']");
			content.RemoveChild(entryOptions);
		}

		private static DateTime GetTimestamp(HtmlDocument html)
		{
			var dateText = html.DocumentNode.SelectSingleNode("//section/article/h3[@class='bp-date']").InnerText;
			return DateTime.ParseExact(dateText, "MMMM d, yyyy", new CultureInfo("en-US", true));
		}

		private IList<Tag> GetTags(HtmlDocument html)
		{
			return null;
		}

		private Meta GetMeta(HtmlDocument html)
		{
			return null;
		}
	}
}