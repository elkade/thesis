using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UniversityWebsite.Controllers;
using UniversityWebsite.Services;

namespace UniversityWebsite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AutofacConfig.ConfigureContainer();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.Configure();
            AutoMapperServiceConfig.Configure();
        }
        protected void Application_Error()
        {

            if (Context.IsCustomErrorEnabled)
                ShowCustomErrorPage(Server.GetLastError());

        }

        private void ShowCustomErrorPage(Exception exception)
        {
            var httpException = exception as HttpException;

            if (httpException != null && (httpException.GetHttpCode() == 404 || httpException.GetHttpCode() != 403))
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", "Page");
                IController controller = new PageController(
                    DependencyResolver.Current.GetService<IPageService>(),
                    DependencyResolver.Current.GetService<ILanguageService>(),
                    DependencyResolver.Current.GetService<IMenuService>(),
                    DependencyResolver.Current.GetService<IDictionaryService>()
                    );
                routeData.Values.Add("action", "Index");
                routeData.Values.Add(Consts.HttpStatusCodeKey, httpException.GetHttpCode());
                controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                Server.ClearError();
            }
        }
    }
}
