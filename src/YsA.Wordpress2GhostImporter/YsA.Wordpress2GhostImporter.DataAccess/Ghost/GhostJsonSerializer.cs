using System;
using Newtonsoft.Json;
using YsA.Wordpress2GhostImporter.DataAccess.Json;

namespace YsA.Wordpress2GhostImporter.DataAccess.Ghost
{
	public class GhostJsonSerializer : IJsonSerializer
	{
		private static readonly JsonSerializerSettings SerializtionSettings = new JsonSerializerSettings
		{
			DateTimeZoneHandling = DateTimeZoneHandling.Utc,
			ContractResolver = new UnderscorePropertyContractResolver(),
			Converters = new JsonConverter[] { new MilisecondsDateConverter() }
		};

		public string Serialize<T>(T target) where T : class
		{
			if (target == null) throw new ArgumentNullException();

			return JsonConvert.SerializeObject(target, SerializtionSettings);
		} 
	}
}