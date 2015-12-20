using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity.Owin;
using UniversityWebsite.Core;
using UniversityWebsite.Filters;
using UniversityWebsite.Services;
using Module = Autofac.Module;

namespace UniversityWebsite
{
    /// <summary>
    /// Odpowiada za konfigurację kontenera Dependency Injection Autofac
    /// </summary>
    public class AutofacConfig
    {
        /// <summary>
        /// Kontener Autofac
        /// </summary>
        public static IContainer Container { get; private set; }
        /// <summary>
        /// Wykonuje konfigurację kontenera Dependency Injection Autofac
        /// </summary>
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterFilterProvider();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterModule(new EfModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new FiltersModule());

            Container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }

        private class ServiceModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<DictionaryService>().As<IDictionaryService>().InstancePerRequest();
                builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
                builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerRequest();
                builder.RegisterType<MenuService>().As<IMenuService>().InstancePerRequest();
                builder.RegisterType<PageService>().As<IPageService>().InstancePerRequest();
                builder.RegisterType<SubjectService>().As<ISubjectService>().InstancePerRequest();
                //builder.RegisterType<LocalPhotoManager>().As<IPhotoManager>().InstancePerRequest();
                base.Load(builder);
            }
        }

        private class EfModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType(typeof(DomainContext)).As(typeof(IDomainContext)).InstancePerRequest();
                
                //builder.RegisterType<UserStore<User>>().As<IUserStore<User>>().InstancePerRequest();

                builder.Register(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()).InstancePerRequest();

                //builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
                //builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
                //builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
                base.Load(builder);
            }
        }
        private class FiltersModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType(typeof(LanguageFilterAttribute)).AsActionFilterFor<Controller>().InstancePerRequest();
                base.Load(builder);
            }
        }
    }
}
