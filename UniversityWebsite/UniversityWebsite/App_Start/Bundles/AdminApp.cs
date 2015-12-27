using System.Web.Optimization;

namespace UniversityWebsite.Bundles
{
    public class AdminApp
    {
        public static Bundle Styles()
        {
            return new StyleBundle("~/Content/AdminApp").Include(
                       "~/Content/angular-busy.css");
        }

        public static Bundle Scripts()
        {
            return new ScriptBundle("~/bundles/configApp")
                .Include("~/AdminApp/libs/angular/angular.min.js")
                .Include("~/AdminApp/libs/angular/angular-route.min.js")
                .Include("~/AdminApp/libs/angular/angular-animate.min.js")
                .Include("~/AdminApp/libs/ui-router/angular-ui-router.min.js")
                .Include("~/AdminApp/libs/angular/angular-resource.min.js")
                .Include("~/AdminApp/libs/file-upload/ng-file-upload-all.js")
                .Include("~/AdminApp/libs/pagination/dirPagination.js")
                .Include("~/AdminApp/libs/spinner/spin.js")
                .Include("~/AdminApp/libs/spinner/angular-spinner.js")
                .IncludeDirectory("~/AdminApp/common", "*.js")
                .IncludeDirectory("~/AdminApp/services", "*.js")
                .IncludeDirectory("~/AdminApp/libs", "*.js")
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
                .IncludeDirectory("~/AdminApp/users/controllers", "*.js")
                .IncludeDirectory("~/AdminApp/gallery", "*.js")
                .IncludeDirectory("~/AdminApp/gallery/controllers", "*.js")
                .Include("~/AdminApp/app.js");
        }
    }
}