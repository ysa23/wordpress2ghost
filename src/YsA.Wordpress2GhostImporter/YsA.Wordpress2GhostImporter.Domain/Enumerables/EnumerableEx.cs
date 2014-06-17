using System.Collections.Generic;
using System.Linq;

namespace YsA.Wordpress2GhostImporter.Domain.Enumerables
{
	public static class EnumerableEx
	{
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> target)
		{
			return target ?? Enumerable.Empty<T>();
		}
	}
}