using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace YsA.Wordpress2GhostImporter.DataAccess.Json
{
	public class UnderscorePropertyContractResolver : DefaultContractResolver
	{
		private static readonly Regex UnderscorePattern = new Regex("([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", RegexOptions.Compiled);

		protected override string ResolvePropertyName(string propertyName)
		{
			return UnderscorePattern.Replace(propertyName, "$1$3_$2$4").ToLower();
		}
	}
}