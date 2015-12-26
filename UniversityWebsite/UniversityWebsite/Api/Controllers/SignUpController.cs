using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Api.Model.Teaching;
using UniversityWebsite.Domain.Model;
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
        public IHttpActionResult GetRequests(int subjectId, int? limit = null, int? offset = null)
        {
            var requests = _subjectService.GetAllRequests(subjectId, limit.Value, offset.Value).ToList();
            
            var result = requests.Select(r => new RequestVm
            {
                Id = r.Id,
                StudentFirstName = r.Student.FirstName,
                StudentLastName = r.Student.LastName,
                StudentId = r.StudentId,
                StudentIndex = r.Student.IndexNumber,
                SubjectTitle = r.Subject.Name,
                SubjectUrlName = r.Subject.UrlName,
                SubjectId = r.SubjectId,
                Status = r.Status.ToString()
            }).ToList();

            return Ok(result);
        }

        [Limit(50), Offset]
        [Route("count")]
        public IHttpActionResult GetRequestsNumber()
        {
            var userId = User.Identity.GetUserId();
            int number = _subjectService.GetRequestsNumberByTeacher(userId);
            return Ok(number);
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
        public IHttpActionResult ApproveRefuse(int[] requestIds)
        {
            foreach (var requestId in requestIds)
            {
                _subjectService.RefuseRequest(requestId);
            }
            return Ok();
        }

        //[Route("subjects/{subjectId:int}/students")]
        //public IHttpActionResult GetStudents(int subjectId, int? limit = null, int? offset = null)
        //{
        //    var result = _subjectService.GetStudents(subjectId, limit ?? 50, offset ?? 0)
        //        .Select(s => new UserReturnModel
        //        {
        //            FirstName = s.FirstName,
        //            LastName = s.LastName,
        //            Email = s.Email
        //        });
        //    return Ok(result);
        //}
    }
}