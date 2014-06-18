using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using YsA.Wordpress2GhostImporter.DataAccess.Net;
using YsA.Wordpress2GhostImporter.Domain.Blog;

namespace YsA.Wordpress2GhostImporter.Ioc
{
	public class ConsoleInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Classes.FromAssemblyContaining<Post>().Pick()
					.WithService.DefaultInterfaces()
					.Configure(s => s.LifestyleSingleton()));
			container.Register(
				Classes.FromAssemblyContaining<HtmlReader>().Pick()
					.WithService.DefaultInterfaces()
					.Configure(s => s.LifestyleSingleton()));
		}
	}
}
