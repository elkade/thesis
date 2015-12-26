using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace UniversityWebsite.Filters
{
    public class LimitAttribute : ActionFilterAttribute
    {
        private readonly int _max;

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