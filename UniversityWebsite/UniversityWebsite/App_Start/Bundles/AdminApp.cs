using System.Web.Optimization;

namespace UniversityWebsite.Bundles
{
    public class AdminApp
    {
        public static Bundle Scripts()
        {
            return new ScriptBundle("~/bundles/configApp")
                .IncludeDirectory("~/AdminApp/common", "*.js")
                .IncludeDirectory("~/AdminApp/modules", "*.js")
                .IncludeDirectory("~/AdminApp/pages", "*.js")
                .IncludeDirectory("~/AdminApp/pages/controllers", "*.js")
                .IncludeDirectory("~/AdminApp/subjects", "*.js")
                .IncludeDirectory("~/AdminApp/subjects/controllers", "*.js")
                .IncludeDirectory("~/AdminApp/menus", "*.js")
                .IncludeDirectory("~/AdminApp/menus/controllers", "*.js")
                .IncludeDirectory("~/AdminApp/main-pages", "*.js")
                .IncludeDirectory("~/AdminApp/main-pages/controllers", "*.js")
                .IncludeDirectory("~/AdminApp/languages", "*.js")
                .IncludeDirectory("~/AdminApp/languages/controllers", "*.js")
                .IncludeDirectory("~/AdminApp/users", "*.js")
                .Include("~/AdminApp/app.js");
        }
    }
}