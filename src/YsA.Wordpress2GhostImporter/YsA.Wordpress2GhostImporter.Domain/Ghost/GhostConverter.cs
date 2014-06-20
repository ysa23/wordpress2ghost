using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using YsA.Wordpress2GhostImporter.Domain.Blog;
using YsA.Wordpress2GhostImporter.Domain.Enumerables;
using YsA.Wordpress2GhostImporter.Domain.Time;

namespace YsA.Wordpress2GhostImporter.Domain.Ghost
{
	public interface IGhostConverter
	{
		GhostImport FromPosts(IList<Post> posts);
	}

	public class GhostConverter : IGhostConverter
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public GhostConverter(IDateTimeProvider dateTimeProvider)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public GhostImport FromPosts(IList<Post> posts)
		{
			if (posts == null) throw new ArgumentNullException("posts");

			if (posts.Count == 0)
				return null;

			var postsWithTags =
				posts.Select((x, i) => new {GhostPost = FromPost(x, i), Tags = x.Tags.EmptyIfNull().Select(t => t.Name)}).ToArray();
			var tags = GetTagsFromPosts(posts);

			return new GhostImport(new GhostImportData
			{
				Posts = postsWithTags.Select(x => x.GhostPost).ToArray(),
				Tags = tags,
				PostTags = postsWithTags
					.SelectMany(x => x.Tags
						.Select(t => new { Tag = t, Post = x.GhostPost }))
					.Join(tags, x => x.Tag, x => x.Name, (p, t) => new PostTag { PostId = p.Post.Id, TagId = t.Id })
					.ToArray()
			});
		}

		private GhostTag[] GetTagsFromPosts(IList<Post> posts)
		{
			return posts.SelectMany(x => 
					x.Tags.EmptyIfNull().Select(t => t.Name))
				.Distinct()
				.OrderBy(x => x)
				.Select(GetTag)
				.ToArray();
		}

		private GhostTag GetTag(string name, int index)
		{
			return new GhostTag
			{
				Id = index + 1,
				CreatedAt = _dateTimeProvider.Now(),
				Name = name
			};
		}

		private static GhostPost FromPost(Post post, int index)
		{
			var sanitisedContent = post.Content.Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty);
			
			return new GhostPost
			{
				Id = index + 1,
				Title = post.Title,
				Html = sanitisedContent,
				Markdown = sanitisedContent,
				CreatedAt = post.Timestamp,
				PublishedAt = post.Timestamp,
				MetaTitle = post.Meta == null ? null : post.Meta.Title,
				MetaDescription = post.Meta == null ? null : post.Meta.Description
			};
		}
	}
}