using System;
using NUnit.Framework;
using Rhino.Mocks;
using YsA.Wordpress2GhostImporter.Domain.Blog;
using YsA.Wordpress2GhostImporter.Domain.Ghost;
using YsA.Wordpress2GhostImporter.Domain.Time;

namespace YsA.Wordpress2GhostImporter.Tests.Domain.Ghost
{
	public class GhostConverterTests
	{
		private IGhostConverter _target;
		private IDateTimeProvider _dateTimeProvider;

		[SetUp]
		public void Setup()
		{
			_dateTimeProvider = MockRepository.GenerateStub<IDateTimeProvider>();

			_target = new GhostConverter(_dateTimeProvider);
		}

		[Test]
		public void FromPosts_WhenListOfPostsIsNull_ThrowException()
		{
			Assert.Throws<ArgumentNullException>(() => _target.FromPosts(null));
		}

		[Test]
		public void FromPosts_WhenListOfPostsIsEmpty_ReturnNull()
		{
			var result = _target.FromPosts(new Post[0]);

			Assert.That(result, Is.Null);
		}

		[Test]
		public void FromPosts_WhenPostsHasNoTags_ConvertOnlyPostsWithoutTags()
		{
			var posts = new[]
			{
				new Post { Title = "post1", Content = "<p>first content</p>", Tags = null, Timestamp = new DateTime(2014, 1, 2) },
				new Post { Title = "post2", Content = "<p>second content</p>", Tags = null, Timestamp = new DateTime(2014, 4, 5) }
			};

			var result = _target.FromPosts(posts);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Posts, Has.Length.EqualTo(2));
			Assert.That(result.Posts, Has.Some.Matches<GhostPost>(x =>
				x.Id == 1 &&
				x.Title == "post1" &&
				x.Html == "<p>first content</p>" &&
				x.Markdown == "<p>first content</p>" &&
				x.MetaTitle == null &&
				x.MetaDescription == null &&
				x.CreatedAt == new DateTime(2014, 1, 2) &&
				x.PublishedAt == new DateTime(2014, 1, 2)));
			Assert.That(result.Posts, Has.Some.Matches<GhostPost>(x =>
				x.Id == 2 &&
				x.Title == "post2" &&
				x.Html == "<p>second content</p>" &&
				x.Markdown == "<p>second content</p>" &&
				x.MetaTitle == null &&
				x.MetaDescription == null &&
				x.CreatedAt == new DateTime(2014, 4, 5) &&
				x.PublishedAt == new DateTime(2014, 4, 5)));
			Assert.That(result.Tags, Is.Empty, "Tags should be empty");
			Assert.That(result.PostTags, Is.Empty, "PostTags should be empty");
		}

		[Test]
		public void FromPosts_WhenPostsHasTags_SetTagsAndPostTags()
		{
			var now = new DateTime(2014, 6, 7);
			SetNow(now);

			var posts = new[]
			{
				new Post { Title = "post1", Content = "<p>first content</p>", Tags = new [] { new Tag { Name = "tag1" }, new Tag { Name = "tag2" } }, Timestamp = new DateTime(2014, 1, 2) },
				new Post { Title = "post2", Content = "<p>second content</p>", Tags = new [] { new Tag { Name = "tag3" }, new Tag { Name = "tag4" } }, Timestamp = new DateTime(2014, 4, 5) }
			};

			var result = _target.FromPosts(posts);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Posts, Has.Length.EqualTo(2));
			Assert.That(result.Tags, Has.Length.EqualTo(4));
			Assert.That(result.Tags, Has.Some.Matches<GhostTag>(x =>
				x.Id == 1 &&
				x.Name == "tag1" &&
				x.CreatedAt == now
			));
			Assert.That(result.Tags, Has.Some.Matches<GhostTag>(x =>
				x.Id == 2 &&
				x.Name == "tag2" &&
				x.CreatedAt == now
			));
			Assert.That(result.Tags, Has.Some.Matches<GhostTag>(x =>
				x.Id == 3 &&
				x.Name == "tag3" &&
				x.CreatedAt == now
			));
			Assert.That(result.Tags, Has.Some.Matches<GhostTag>(x =>
				x.Id == 4 &&
				x.Name == "tag4" &&
				x.CreatedAt == now
			));
			Assert.That(result.PostTags, Has.Length.EqualTo(4));
			Assert.That(result.PostTags, Has.Some.Matches<PostTag>(x => x.PostId == 1 && x.TagId == 1));
			Assert.That(result.PostTags, Has.Some.Matches<PostTag>(x => x.PostId == 1 && x.TagId == 2));
			Assert.That(result.PostTags, Has.Some.Matches<PostTag>(x => x.PostId == 2 && x.TagId == 3));
			Assert.That(result.PostTags, Has.Some.Matches<PostTag>(x => x.PostId == 2 && x.TagId == 4));
		}

		[Test]
		public void FromPosts_WhenPostsContainsTheSameTags_CreateOnlyOneInstaceOfSharedTags()
		{
			SetNow(new DateTime(2014, 6, 7));

			var posts = new[]
			{
				new Post { Title = "post1", Content = "<p>first content</p>", Tags = new [] { new Tag { Name = "tag1" }, new Tag { Name = "tag2" } }, Timestamp = new DateTime(2014, 1, 2) },
				new Post { Title = "post2", Content = "<p>second content</p>", Tags = new [] { new Tag { Name = "tag1" }, new Tag { Name = "tag3" } }, Timestamp = new DateTime(2014, 4, 5) }
			};

			var result = _target.FromPosts(posts);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Posts, Has.Length.EqualTo(2));
			Assert.That(result.Tags, Has.Length.EqualTo(3));
			Assert.That(result.Tags, Has.Some.Matches<GhostTag>(x =>
				x.Id == 1 &&
				x.Name == "tag1"
			));
			Assert.That(result.Tags, Has.Some.Matches<GhostTag>(x =>
				x.Id == 2 &&
				x.Name == "tag2"
			));
			Assert.That(result.Tags, Has.Some.Matches<GhostTag>(x =>
				x.Id == 3 &&
				x.Name == "tag3"
			));
			Assert.That(result.PostTags, Has.Length.EqualTo(4));
			Assert.That(result.PostTags, Has.Some.Matches<PostTag>(x => x.PostId == 1 && x.TagId == 1));
			Assert.That(result.PostTags, Has.Some.Matches<PostTag>(x => x.PostId == 1 && x.TagId == 2));
			Assert.That(result.PostTags, Has.Some.Matches<PostTag>(x => x.PostId == 2 && x.TagId == 1));
			Assert.That(result.PostTags, Has.Some.Matches<PostTag>(x => x.PostId == 2 && x.TagId == 3));
		}

		[Test]
		public void FromPosts_WhenPostHasMeta_SetMetaTitleAndDescription()
		{
			SetNow(new DateTime(2014, 1, 2));

			var posts = new[]
			{
				new Post { Title = "post1", Content = "<p>first content</p>", Tags = null, Timestamp = new DateTime(2014, 1, 2), Meta = new Meta { Title = "meta title", Description = "meta description" }},
				new Post { Title = "post2", Content = "<p>second content</p>", Tags = null, Timestamp = new DateTime(2014, 4, 5) }
			};

			var result = _target.FromPosts(posts);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Posts, Has.Length.EqualTo(2));
			Assert.That(result.Posts, Has.Some.Matches<GhostPost>(x => x.Id == 1 && x.MetaTitle == "meta title" && x.MetaDescription == "meta description"));
		}

		private void SetNow(DateTime dateTime)
		{
			_dateTimeProvider
				.Stub(x => x.Now())
				.Return(dateTime);
		}
	}
}