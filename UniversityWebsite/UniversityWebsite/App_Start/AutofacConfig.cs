using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
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

            builder.RegisterModule(new ServiceModule());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
        public class ServiceModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<TilesServiceMock>().As<ITilesService>().InstancePerRequest();

                base.Load(builder);
            }
        }
    }
}