namespace YsA.Wordpress2GhostImporter.Domain.Net
{
	public interface IJsonSerializer
	{
		string Serialize<T>(T target) where T : class;
	}
}