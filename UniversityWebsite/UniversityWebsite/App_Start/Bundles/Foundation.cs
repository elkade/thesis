using System.Web.Optimization;

namespace UniversityWebsite.Bundles
{
    /// <summary>
    /// Odpowiada za konfiguracjê grup danych dotycz¹cych frameworka Foundation przesy³anych przy ¿¹daniu HTTP.
    /// </summary>

    public static class Foundation
    {
        /// <summary>
        /// Konfiguruje grupy styli dotycz¹cych frameworka Foundation przesy³anych przy ¿¹daniu HTTP.
        /// </summary>
        /// <returns>Grupa styli</returns>
        public static Bundle Styles()
        {
            return new StyleBundle("~/Content/foundation/css").Include( 
                       "~/Content/font-awesome.min.css", 
                       "~/sass/Site.css");
        }
        /// <summary>
        /// Konfiguruje grupy skryptów dotycz¹cych frameworka Foundation przesy³anych przy ¿¹daniu HTTP.
        /// </summary>
        /// <returns>Grupa skryptów</returns>
        public static Bundle Scripts()
        {
            return new ScriptBundle("~/bundles/foundation").Include(
                      "~/Scripts/foundation/fastclick.js",
                      "~/Scripts/jquery.cookie.js",
                      "~/Scripts/foundation/foundation.js",
                      "~/Scripts/foundation/foundation.*");
        }
    }
}