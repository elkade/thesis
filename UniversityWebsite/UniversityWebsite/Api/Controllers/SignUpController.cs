using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Api.Model;
using UniversityWebsite.Api.Model.Teaching;
using UniversityWebsite.Filters;
using UniversityWebsite.Services;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/signup")]
    //[Authorize(Roles = Consts.TeacherRole)]
    public class SignUpController : ApiController
    {
        private readonly ISubjectService _subjectService;

        public SignUpController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Limit(50),Offset]
        [Route("")]
        [HttpGet]
        public PaginationVm<RequestVm> GetRequestsBySubject(int subjectId, int limit = 50, int offset = 0)
        {
            var requests = _subjectService.GetRequestsBySubject(subjectId, limit, offset);

            int number = _subjectService.GetRequestsNumberBySubject(subjectId);

            return new PaginationVm<RequestVm>(Mapper.Map<List<RequestVm>>(requests),number,limit,offset);
        }


        [Limit(50), Offset]
        [Route("")]
        [HttpGet]
        //[Authorize(Roles=Consts.TeacherRole)]
        public PaginationVm<RequestVm> GetRequestsByTeacher(int limit = 50, int offset = 0)
        {
            var userId = User.Identity.GetUserId();

            var requests = _subjectService.GetRequestsByTeacher(userId, limit, offset);

            int number = _subjectService.GetRequestsNumberByTeacher(userId);

            return new PaginationVm<RequestVm>(Mapper.Map<List<RequestVm>>(requests), number, limit, offset);
        }

        [Route("approve")]
        [HttpPost]
        public IHttpActionResult ApproveRequest(int[] requestIds)
        {
            foreach (var requestId in requestIds)
            {
                _subjectService.ApproveRequest(requestId);   
            }
            return Ok();
        }   

        [Route("reject")]
        [HttpPost]
        public IHttpActionResult RefuseRequest(int[] requestIds)
        {
            foreach (var requestId in requestIds)
            {
                _subjectService.RefuseRequest(requestId);
            }
            return Ok();
        }
    }
}