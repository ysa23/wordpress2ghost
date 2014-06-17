using System;

namespace YsA.Wordpress2GhostImporter.Domain.Time
{
	public interface IDateTimeProvider
	{
		DateTime Now();
	}

	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime Now()
		{
			return DateTime.UtcNow;
		}
	}
}