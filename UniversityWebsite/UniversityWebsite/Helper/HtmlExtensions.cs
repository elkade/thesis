using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace UniversityWebsite.Helper
{
    public static class HtmlExtensions
    {
        public static void RenderPhrase(this HtmlHelper helper, string id, string l)
        {
            helper.RenderAction("Index", "Dictionary", new {id = id, l = l});
        }
    }
}