using System.Web;
using System.Web.Mvc;
using UniversityWebsite.Services;

namespace UniversityWebsite.Filters
{
    /// <summary>
    /// Filtr odpowiedzialny za obsługę języka, w jakim wyświetlane są strony systemu.
    /// </summary>
    public class LanguageFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        private readonly ILanguageService _languageService;

        /// <summary>
        /// Tworzy nową instancję filtru
        /// </summary>
        /// <param name="languageService">Serwis odpowiedzialny za zarządzanie językami systemu</param>
        public LanguageFilterAttribute(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var session = context.HttpContext.Session;

            string lang = context.HttpContext.Request.Unvalidated[Consts.ParamKeyLang];

            if (!string.IsNullOrEmpty(lang) && _languageService.Exists(lang))
            {
                session[Consts.SessionKeyLang] = lang;
                SetCookie(Consts.CookieKeyLang, lang, context.HttpContext.Response);
                return;
            }
            if (session[Consts.SessionKeyLang] != null) return;
            lang = GetCookie(Consts.CookieKeyLang, context.HttpContext.Request);
            if (_languageService.Exists(lang))
            {
                session[Consts.SessionKeyLang] = lang;
                return;
            }
            session[Consts.SessionKeyLang] = Consts.DefaultLanguage;
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        private void SetCookie(string key, string value, HttpResponseBase response)
        {
            var encodedValue = HttpUtility.UrlEncode(value);
            var cookie = new HttpCookie(key, encodedValue)
            {
                HttpOnly = true,
            };
            response.AppendCookie(cookie);
        }

        private string GetCookie(string key, HttpRequestBase request)
        {
            var cookie = request.Cookies[key];
            if (cookie == null)
                return null;
            var decodedValue = HttpUtility.UrlDecode(cookie.Value);
            return decodedValue;
        }
    }
}