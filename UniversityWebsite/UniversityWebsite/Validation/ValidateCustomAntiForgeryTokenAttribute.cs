using System;
using System.Linq;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http.Filters;

namespace UniversityWebsite.Validation
{
    public sealed class ValidateCustomAntiForgeryTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            var headers = actionContext.Request.Headers;
            var cookie = headers
                .GetCookies()
                .Select(c => c[AntiForgeryConfig.CookieName])
                .FirstOrDefault();
            var tokenFromHeader = "";
            if (headers.Contains("X-XSRF-Token"))
                tokenFromHeader = headers.GetValues("X-XSRF-Token").FirstOrDefault();
            AntiForgery.Validate(cookie != null ? cookie.Value : null, tokenFromHeader);

            base.OnActionExecuting(actionContext);
        }
    }
}