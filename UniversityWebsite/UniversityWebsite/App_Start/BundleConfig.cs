using System.Web.Optimization;

namespace UniversityWebsite
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(Foundation.Styles());
            bundles.Add(Foundation.Scripts());

            bundles.Add(new StyleBundle("~/tinymce/css")
                .IncludeDirectory("~/Scripts/tinymce/skins/lightgray", "*.css"));

            bundles.Add(new ScriptBundle("~/bundles/configApp")
                .IncludeDirectory("~/Scripts/app/common", "*.js")
                .IncludeDirectory("~/Scripts/app/modules", "*.js")
                .IncludeDirectory("~/Scripts/app/pages", "*.js")
                .IncludeDirectory("~/Scripts/app/pages/controllers", "*.js")
                .IncludeDirectory("~/Scripts/app/subjects", "*.js")
                .IncludeDirectory("~/Scripts/app/menus", "*.js")
                .IncludeDirectory("~/Scripts/app/main-pages", "*.js")
                .IncludeDirectory("~/Scripts/app/users", "*.js")
                .Include("~/Scripts/app/app.js"));
        }  
    }
}