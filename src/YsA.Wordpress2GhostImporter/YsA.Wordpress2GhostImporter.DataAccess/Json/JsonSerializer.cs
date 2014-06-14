using System;
using Newtonsoft.Json;

namespace YsA.Wordpress2GhostImporter.DataAccess.Json
{
	public interface IJsonSerializer
	{
		string Serialize<T>(T target) where T : class;
	}

	public class JsonSerializer : IJsonSerializer
	{
		private static readonly JsonSerializerSettings SerializtionSettings = new JsonSerializerSettings
		{
			DateTimeZoneHandling = DateTimeZoneHandling.Utc,
			ContractResolver = new UnderscorePropertyContractResolver(),
			Converters = new JsonConverter[] {new MilisecondsDateConverter()}
		};

		public string Serialize<T>(T target) where T:class
		{
			if (target == null) throw new ArgumentNullException();

			return JsonConvert.SerializeObject(target, SerializtionSettings);
		}
	}
}