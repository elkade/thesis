using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Services;

namespace UniversityWebsite.Filters
{
    public class ValidateWriteAccessToSubjectAttribute : ActionFilterAttribute, IActionFilter
    {
        private readonly ISubjectService _subjectService;

        public ValidateWriteAccessToSubjectAttribute()
        {
            _subjectService = DependencyResolver.Current.GetService<ISubjectService>();
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!context.ActionParameters.ContainsKey("subjectId"))
                throw new Exception("No subjectId parameter in request.");
            var i = context.ActionParameters["subjectId"] as int?;
            if (i == null)
                throw new Exception("subjectId parameter in request is empty.");
            int subjectId = i.Value;
            if (HttpContext.Current.User.IsInRole(Consts.AdministratorRole))
                return;
            if (!HttpContext.Current.User.IsInRole(Consts.TeacherRole))
                throw new UnauthorizedAccessException();
            if (_subjectService.HasTeacherAccessToSubject(HttpContext.Current.User.Identity.GetUserId(), subjectId))
                return;
            throw new UnauthorizedAccessException();
        }

    }
}