using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Api.Model.Teaching;
using UniversityWebsite.Filters;
using UniversityWebsite.Services;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/signup")]
    [Authorize(Roles = Consts.TeacherRole)]
    public class SignUpController : ApiController
    {
        private readonly ISubjectService _subjectService;

        public SignUpController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Limit(50),Offset]
        [Route("")]
        public IEnumerable<RequestVm> GetRequests(int? limit = null, int? offset = null)
        {
            var userId = User.Identity.GetUserId();
           // var requests = _subjectService.GetRequestsByTeacher(userId, limit.Value, offset.Value);
            return null;
        }

        [Limit(50), Offset]
        [Route("count")]
        public IHttpActionResult GetRequestsNumber()
        {
            var userId = User.Identity.GetUserId();
            int number = _subjectService.GetRequestsNumberByTeacher(userId);
            return Ok(number);
        }

        [Route("{id:int}")]
        [HttpPost]
        public IHttpActionResult ApproveRequest(int requestId)
        {
            _subjectService.ApproveRequest(requestId);
            return Ok();
        }

        [Route("{id:int}")]
        [HttpPost]
        public IHttpActionResult ApproveRefuse(int requestId)
        {
            _subjectService.RefuseRequest(requestId);
            return Ok();
        }
    }
}