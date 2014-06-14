using System;
using Newtonsoft.Json;
using YsA.Wordpress2GhostImporter.DataAccess.Json;
using YsA.Wordpress2GhostImporter.Domain.Ghost;

namespace YsA.Wordpress2GhostImporter
{
	class Program
	{
		static void Main()
		{
			var serializationSettings = new JsonSerializerSettings
			{
				DateTimeZoneHandling = DateTimeZoneHandling.Utc,
				Formatting = Formatting.Indented,
				ContractResolver = new UnderscorePropertyContractResolver(),
				Converters = new JsonConverter[] { new MilisecondsDateConverter() }
			};

			var serialized = JsonConvert.SerializeObject(new Post { CreatedAt = DateTime.UtcNow }, serializationSettings);

			Console.WriteLine(serialized);
		}
	}
}
