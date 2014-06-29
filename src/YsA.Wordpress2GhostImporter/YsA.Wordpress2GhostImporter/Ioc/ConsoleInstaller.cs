using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using YsA.HtmlToMarkdown;
using YsA.Wordpress2GhostImporter.DataAccess.Net;
using YsA.Wordpress2GhostImporter.Domain.Blog;
using YsA.Wordpress2GhostImporter.Domain.Writers;
using YsA.Wordpress2GhostImporter.Writers;

namespace YsA.Wordpress2GhostImporter.Ioc
{
	public class ConsoleInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IOutputWriter>().ImplementedBy<ConsoleWriter>().LifestyleSingleton());

			container.Register(
				Classes.FromAssemblyContaining<Post>().Pick()
					.WithService.DefaultInterfaces()
					.Configure(s => s.LifestyleSingleton()));
			container.Register(
				Classes.FromAssemblyContaining<HtmlReader>().Pick()
					.WithService.DefaultInterfaces()
					.Configure(s => s.LifestyleSingleton()));
			container.Register(
				Classes.FromAssemblyContaining<IHtmlToMarkdownConverter>().Pick()
					.WithService.DefaultInterfaces()
					.Configure(s => s.LifestyleSingleton()));
		}
	}
}
