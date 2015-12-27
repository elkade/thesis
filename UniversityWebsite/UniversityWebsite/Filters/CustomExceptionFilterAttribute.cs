using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //if (context.Exception is NotFoundException)
            //{
            //    context.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            //}
            //else
            if (context.Exception is PropertyValidationException)
            {
                var ex = context.Exception as PropertyValidationException;
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                context.Response.Content = new ObjectContent(typeof(object),
                    new{ModelState=new Dictionary<string,string[]>{{ex.PropertyName, new[]{ex.PropertyValidationMessage}}}}, new JsonMediaTypeFormatter() );
            }
        }
    }
}
/*using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.Filters
{
    public class NotFoundExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.NotFound, context.Exception.Message);
            }
        }
    }
}*/