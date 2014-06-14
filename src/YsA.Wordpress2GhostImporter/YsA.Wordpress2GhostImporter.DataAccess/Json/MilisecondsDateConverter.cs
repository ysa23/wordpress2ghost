using System;
using Newtonsoft.Json;

namespace YsA.Wordpress2GhostImporter.DataAccess.Json
{
	public class MilisecondsDateConverter : JsonConverter
	{
		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var dateValue = value is DateTime ? (DateTime) value : ((DateTime?) value).Value;

			writer.WriteValue((long)dateValue.ToUniversalTime().Subtract(Epoch).TotalMilliseconds);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.Value != null) 
				return Epoch.AddMilliseconds(Convert.ToInt64(reader.Value));

			return objectType == typeof (DateTime) ? (object) default(DateTime) : null;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof (DateTime) ||
				   objectType == typeof (DateTime?);
		}
	}
}