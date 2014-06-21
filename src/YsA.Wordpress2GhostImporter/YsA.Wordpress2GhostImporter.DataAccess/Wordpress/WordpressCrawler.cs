using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using YsA.Wordpress2GhostImporter.DataAccess.Net;
using YsA.Wordpress2GhostImporter.Domain.Blog;
using YsA.Wordpress2GhostImporter.Domain.Enumerables;
using YsA.Wordpress2GhostImporter.Domain.Net;
using YsA.Wordpress2GhostImporter.Domain.Wordpress;

namespace YsA.Wordpress2GhostImporter.DataAccess.Wordpress
{
	public class WordpressCrawler : IWordpressCrawler
	{
		private readonly IHtmlReader _htmlReader;

		public WordpressCrawler(IHtmlReader htmlReader)
		{
			_htmlReader = htmlReader;
		}

		public IEnumerable<Post> Crawl(string targetUrl)
		{
			var html = _htmlReader.GetHtmlFromUrl(targetUrl);

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

				html = _htmlReader.GetHtmlFromUrl(nextPageLink.Attributes["href"].Value);
			}
		}

		private Post CrawlPost(string postUrl)
		{
			try
			{
				var html = _htmlReader.GetHtmlFromUrl(postUrl);

				var content = html.DocumentNode.SelectSingleNode("//section/article/div[@class='contx entry-content clearfix']");
				RemoveRedundentContent(content);

				return new Post
				{
					Url = postUrl,
					Title = html.DocumentNode.SelectSingleNode("//section/article/h2/a").InnerText,
					Content = content.InnerHtml,
					Timestamp = GetTimestamp(html),
					Meta = GetMeta(html),
					Tags = GetTags(html),
					Images = GetImages(content)
				};
			}
			catch (Exception ex)
			{
				throw new PostCrawlingException(postUrl, ex);
			}
		}

		private IList<Image> GetImages(HtmlNode content)
		{
			return content
				.SelectNodes("descendant::img")
				.EmptyIfNull()
				.Select(x => new Image(x.Attributes["src"].Value))
				.ToArray();
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
			var nodes = html.DocumentNode.SelectSingleNode("//section/article").SelectNodes("descendant::a[@rel='tag']");

			if (nodes == null || nodes.Count == 0)
				throw new PostIsNotTaggedException();

			return nodes
				.Select(x => new Tag {Name = x.InnerText, Url = x.Attributes["href"].Value})
				.ToArray();
		}

		private Meta GetMeta(HtmlDocument html)
		{
			return new Meta
			{
				Title = GetMetaValue(html, "title"),
				Description = GetMetaValue(html, "description")
			};
		}

		private string GetMetaValue(HtmlDocument html, string metaType)
		{
			var node = html.DocumentNode.SelectSingleNode("//head/meta[@name='" + metaType + "']");

			return node == null
				? null
				: node.Attributes["content"].Value;
		}
	}
}