using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Api.Model.Teaching;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/teaching")]
    public class TeachingController : ApiController
    {
        private readonly ISubjectService _subjectService;

        public TeachingController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        [Route("subjects")]
        //[AntiForgeryValidate]
        public IEnumerable<SubjectDto> GetSubjects(int? offset = null, int? limit = null)//max limit to 50
        {
            return _subjectService.GetSubjects(offset??0, limit??50);
        }
        [Route("subjects")]
        public IHttpActionResult PostSubject(SubjectPost subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subjectDto = Mapper.Map<SubjectDto>(subject);
            subjectDto.UrlName = PrepareUrlName(subject.Name);
            var addedSubject = _subjectService.AddSubject(subjectDto, User.Identity.GetUserId());
            return Ok(addedSubject);
        }
        [Route("subjects")]
        public IHttpActionResult PutSubject(SubjectPut subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subjectDto = Mapper.Map<SubjectDto>(subject);
            subjectDto.UrlName = PrepareUrlName(subject.Name);
            var updatedSubject = _subjectService.UpdateSubject(subjectDto, User.Identity.GetUserId());
            return Ok(updatedSubject);
        }

        private string PrepareUrlName(string name)
        {
            return HttpUtility.UrlEncode(name.Substring(0, 32 > name.Length ? name.Length : 32)); ;
        }
    }
}