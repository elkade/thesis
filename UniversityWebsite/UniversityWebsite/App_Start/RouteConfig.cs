﻿using System.Web.Mvc;
using System.Web.Routing;

namespace UniversityWebsite
{
    /// <summary>
    /// Odpowiada za konfigurację trasowania adresów URL aplikacji
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Konfiguruje routing aplikacji
        /// </summary>
        /// <param name="routes">Kolekcja obiektów trasowania</param>
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
                name: "Subject",
                url: "Teaching/Subject/{name}",
                defaults: new { controller = "Teaching", action = "Subject" }
            );

            routes.MapRoute(
                name: "Semester",
                url: "Teaching/Semester/{number}",
                defaults: new { controller = "Teaching", action = "Semester" }
            );

            routes.MapRoute(
                name: "Teaching",
                url: "Teaching",
                defaults: new { controller = "Teaching", action = "Index" }
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
