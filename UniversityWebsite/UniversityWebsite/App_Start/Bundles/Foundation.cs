using System.Web.Optimization;

namespace UniversityWebsite.Bundles
{
    /// <summary>
    /// Odpowiada za konfiguracj� grup danych dotycz�cych frameworka Foundation przesy�anych przy ��daniu HTTP.
    /// </summary>

    public static class Foundation
    {
        /// <summary>
        /// Konfiguruje grupy styli dotycz�cych frameworka Foundation przesy�anych przy ��daniu HTTP.
        /// </summary>
        /// <returns>Grupa styli</returns>
        public static Bundle Styles()
        {
            return new StyleBundle("~/Content/foundation/css").Include( 
                       "~/Content/font-awesome.min.css", 
                       "~/sass/Site.css");
        }
        /// <summary>
        /// Konfiguruje grupy skrypt�w dotycz�cych frameworka Foundation przesy�anych przy ��daniu HTTP.
        /// </summary>
        /// <returns>Grupa skrypt�w</returns>
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