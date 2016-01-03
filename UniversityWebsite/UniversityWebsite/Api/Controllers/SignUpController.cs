using System.Collections.Generic;
using System.Linq;
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
        [Authorize(Roles=Consts.AdministratorRole + "," + Consts.TeacherRole)]
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
        //[Authorize(Roles=Consts.TeacherRole)]
        public IHttpActionResult ApproveRequest(int[] requestIds)
        {
            var userId = User.Identity.GetUserId();
            _subjectService.ApproveRequests(requestIds.Distinct(), userId);
            return Ok();
        }   

        [Route("reject")]
        [HttpPost]
        //[Authorize(Roles=Consts.TeacherRole)]
        public IHttpActionResult RefuseRequest(int[] requestIds)
        {
            var userId = User.Identity.GetUserId();
            _subjectService.RefuseRequests(requestIds.Distinct(), userId);
            return Ok();
        }
    }
}