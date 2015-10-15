﻿using System.Web.Mvc;
using System.Web.Routing;

namespace UniversityWebsite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Page",
                url: "{pageName}",
                defaults: new { controller = "Page", action = "Index" }
            );

            routes.MapRoute(
                name: "Teaching",
                url: "Teaching/Subject/{subjectName}",
                defaults: new { controller = "Teaching", action = "GetSubject" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
