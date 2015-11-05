using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;
using UniversityWebsite.Services;

namespace UniversityWebsite
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterModule(new EfModule());
            builder.RegisterModule(new ServiceModule());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private class ServiceModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<TilesServiceMock>().As<ITilesService>().InstancePerRequest();
                builder.RegisterType<DictionaryService>().As<IDictionaryService>().InstancePerRequest();
                builder.RegisterType<MenuService>().As<IMenuService>().InstancePerRequest();
                builder.RegisterType<PageService>().As<IPageService>().InstancePerRequest();
                base.Load(builder);
            }
        }

        private class EfModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType(typeof(DomainContext)).As(typeof(IDomainContext)).InstancePerLifetimeScope();
                base.Load(builder);
            }
        }
    }
}
