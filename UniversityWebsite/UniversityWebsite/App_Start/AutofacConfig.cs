using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Core;
using UniversityWebsite.Services;

namespace UniversityWebsite
{
    public class AutofacConfig
    {
        public static IContainer Container { get; private set; }
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterModule(new EfModule());
            builder.RegisterModule(new ServiceModule());

            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }

        private class ServiceModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<DictionaryService>().As<IDictionaryService>().InstancePerRequest();
                builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerRequest();
                builder.RegisterType<MenuService>().As<IMenuService>().InstancePerRequest();
                builder.RegisterType<PageService>().As<IPageService>().InstancePerRequest();
                base.Load(builder);
            }
        }

        private class EfModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType(typeof(DomainContext)).As(typeof(IDomainContext)).InstancePerRequest();
                base.Load(builder);
            }
        }
    }
}
