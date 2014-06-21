namespace YsA.Wordpress2GhostImporter.Domain.Writers
{
	public interface IOutputWriter
	{
		void WriteLine(string value, params object[] args);
	}
}