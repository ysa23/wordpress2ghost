using System;
using NUnit.Framework;
using YsA.Wordpress2GhostImporter.DataAccess.Ghost;
using YsA.Wordpress2GhostImporter.Domain.Net;

namespace YsA.Wordpress2GhostImporter.Tests.DataAccess.Ghost
{
	[TestFixture]
	public class GhostJsonSerializerTests
	{
		private IJsonSerializer _target;

		[SetUp]
		public void Setup()
		{
			_target = new GhostJsonSerializer();
		}

		[Test]
		public void Serialize_WhenTargetIsNull_ThrowException()
		{
			Assert.Throws<ArgumentNullException>(() => _target.Serialize<SerializationMock>(null));
		}

		[Test]
		public void Serialize_WhenTargetHasPropertiesNullValues_PropertiesAreSerialized()
		{
			var objectToSerialize = new SerializationMock
			{
				Name = null,
				PublishedAt = null,
				CreatedAt = new DateTime(2014, 6, 14,0, 0, 0, DateTimeKind.Utc)
			};

			var result = _target.Serialize(objectToSerialize);

			Assert.That(result, Is.EqualTo("{\"name\":null,\"created_at\":1402704000000,\"published_at\":null}"));
		}

		[Test]
		public void Serialize_WhenTargetHasPropertiesWithMoreThenOneWord_SerializePropertyNameUnderscoreSeperated()
		{
			var objectToSerialize = new SerializationMock
			{
				Name = "test",
				CreatedAt = new DateTime(2014, 6, 14, 0, 0, 0, DateTimeKind.Utc)
			};

			var result = _target.Serialize(objectToSerialize);

			Assert.That(result, Is.EqualTo("{\"name\":\"test\",\"created_at\":1402704000000,\"published_at\":null}"));
		}

		[Test]
		public void Serialize_WhenTargetHasDateTimeProperties_SerializeValueAsMiliseconds()
		{
			var objectToSerialize = new SerializationMock
			{
				Name = "test",
				CreatedAt = new DateTime(2014, 6, 14, 0, 0, 0, DateTimeKind.Utc),
				PublishedAt = new DateTime(2014, 6, 14, 23, 28, 0, DateTimeKind.Utc)
			};

			var result = _target.Serialize(objectToSerialize);

			Assert.That(result, Is.EqualTo("{\"name\":\"test\",\"created_at\":1402704000000,\"published_at\":1402788480000}"));
		}

		public class SerializationMock
		{
			public string Name { get; set; }
			public DateTime CreatedAt { get; set; }
			public DateTime? PublishedAt { get; set; }
		}
	}
}