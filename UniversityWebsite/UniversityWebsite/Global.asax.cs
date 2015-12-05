using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UniversityWebsite.Services;

namespace UniversityWebsite
{
    public class MvcApplication : System.Web.HttpApplication
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
        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    // Code that runs when a new session is started
        //    Session.Add(LanguageModule.Constants.SessionLanguage,
        //      System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
        //}
    }
}
