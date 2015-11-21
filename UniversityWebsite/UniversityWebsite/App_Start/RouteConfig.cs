using System.Web.Mvc;
using System.Web.Routing;

namespace UniversityWebsite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "Home",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin",
                url: "Admin",
                defaults: new { controller = "Admin", action = "Index" }
            );

            routes.MapRoute(
                name: "Teaching",
                url: "Teaching/Subject/{subjectName}",
                defaults: new { controller = "Teaching", action = "GetSubject" }
            );

            routes.MapRoute(
                name: "PageEdit",
                url: "Page/Edit/{pageName}",
                defaults: new { controller = "Page", action = "Edit" }
                );

            routes.MapRoute(
                name: "Page",
                url: "{name}",
                defaults: new { controller = "Page", action = "Index" }
                );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
