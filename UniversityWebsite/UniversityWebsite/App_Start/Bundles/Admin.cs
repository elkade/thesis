using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace UniversityWebsite.App_Start.Bundles
{
    public class Admin
    {
        public static Bundle Styles()
        {
            return new StyleBundle("~/Content/Admin/css").Include(
                       "~/Content/admin/climacons.css",
                       "~/Content/admin/component.css",
                       "~/Content/admin/default.css");
        }

        public static Bundle Scripts()
        {
            return new ScriptBundle("~/bundles/Admin").Include(
                      //"~/Scripts/admin/modernizr.custom.js",
                      "~/Scripts/admin/boxgrid.js",
                      "~/Scripts/admin/jquery.fittext.js"
                      );
        }
    }
}