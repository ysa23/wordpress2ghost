using System.Collections.Generic;
using YsA.Wordpress2GhostImporter.Domain.Blog;
using YsA.Wordpress2GhostImporter.Domain.Net;

namespace YsA.Wordpress2GhostImporter.Domain.Wordpress
{
	public interface IWordpressCrawler : ICrawler<IEnumerable<Post>>
	{
	}
}