namespace YsA.Wordpress2GhostImporter.DataAccess.Json
{
	public interface IJsonSerializer
	{
		string Serialize<T>(T target) where T : class;
	}
}