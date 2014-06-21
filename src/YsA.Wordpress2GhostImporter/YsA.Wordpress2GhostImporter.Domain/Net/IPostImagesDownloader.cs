using System.Collections.Generic;
using YsA.Wordpress2GhostImporter.Domain.Blog;

namespace YsA.Wordpress2GhostImporter.Domain.Net
{
	public interface IPostImagesDownloader
	{
		void DownloadImages(IList<Post> posts, string targetFolder);
	}
}