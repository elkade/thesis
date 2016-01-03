using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace UniversityWebsite.Filters
{
    /// <summary>
    /// Filtr odpowiedzialny za validację i ograniczenie wartości parametru "limit" w żądainu HTTP
    /// </summary>
    public class LimitAttribute : ActionFilterAttribute
    {
        private readonly int _max;

        /// <summary>
        /// Tworzy nową instancję filtru.
        /// </summary>
        /// <param name="max">MAksymalna wartość parametru "limit", do której param. zostaje zawężony, gdy ją przekracza.</param>
        public LimitAttribute(int max)
        {
            _max = max;
        }

        private const string Key = "limit";
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            if (actionContext.ActionArguments.ContainsKey(Key))
            {
                var limit = actionContext.ActionArguments[Key] as int?;
                if (limit == null || limit > _max)
                    actionContext.ActionArguments[Key] = _max;
                else if (limit < 0)
                    actionContext.ActionArguments[Key] = 0;
            }
            else
                actionContext.ActionArguments[Key] = _max;
        }
    }

    /// <summary>
    /// Filtr odpowiedzialny za validację i ograniczenie wartości parametru "offset" w żądainu HTTP
    /// </summary>
    public class OffsetAttribute : ActionFilterAttribute
    {
        private const string Key = "offset";
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            if (actionContext.ActionArguments.ContainsKey(Key))
            {
                var offset = actionContext.ActionArguments[Key] as int?;
                if (offset == null || offset < 0)
                    actionContext.ActionArguments[Key] = 0;
            }
            else
                actionContext.ActionArguments[Key] = 0;
        }
    }
}