using System;
using YsA.Wordpress2GhostImporter.Domain.Writers;

namespace YsA.Wordpress2GhostImporter.Writers
{
	public class ConsoleWriter : IOutputWriter
	{
		public void WriteLine(string value, params object[] args)
		{
			Console.WriteLine(value, args);
		}
	}
}