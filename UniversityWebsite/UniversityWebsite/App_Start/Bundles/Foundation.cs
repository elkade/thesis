using System.Web.Optimization;

namespace UniversityWebsite.Bundles
{
    public static class Foundation
    {
        public static Bundle Styles()
        {
            return new StyleBundle("~/Content/foundation/css").Include(
                       "~/Content/foundation/foundation-icons.css", 
                       "~/sass/Site.css");
        }

        public static Bundle Scripts()
        {
            return new ScriptBundle("~/bundles/foundation").Include(
                      "~/Scripts/foundation/fastclick.js",
                      "~/Scripts/jquery.cookie.js",
                      "~/Scripts/foundation/foundation.js",
                      "~/Scripts/foundation/foundation.*",
                      "~/Scripts/foundation/app.js");
        }
    }
}