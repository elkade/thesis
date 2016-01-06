using System.Web.Optimization;
using UniversityWebsite.Bundles;

namespace UniversityWebsite
{
    /// <summary>
    /// Odpowiada za konfigurację grupowania danych wysyłanych przy żądaniach HTTP.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Rejestruje grupy danych wysyłanych przy żądaniach HTTP.
        /// </summary>
        /// <param name="bundles">Obiekt reprezentujący grupy danych</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
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

            bundles.Add(new ScriptBundle("~/bundles/linq").Include(
                "~/Scripts/linq.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(Foundation.Styles());
            bundles.Add(Foundation.Scripts());

            bundles.Add(new StyleBundle("~/tinymce/css")
                .IncludeDirectory("~/Scripts/tinymce/skins/lightgray", "*.css"));

            bundles.Add(AdminApp.Scripts());
            bundles.Add(AdminApp.Styles());
        }  
    }
}