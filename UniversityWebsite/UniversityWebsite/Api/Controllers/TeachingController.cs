using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Api.Model;
using UniversityWebsite.Api.Model.Teaching;
using UniversityWebsite.Filters;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/subjects")]
    public class TeachingController : ApiController
    {
        private readonly ISubjectService _subjectService;
        private readonly IUserService _userService;

        public TeachingController(ISubjectService subjectService, IUserService userService)
        {
            _subjectService = subjectService;
            _userService = userService;
        }

        [Limit(50), Offset]
        [Route("")]
        [Authorize(Roles = "Administrator")]
        //[AntiForgeryValidate]
        public PaginationVm<SubjectDto> GetSubjects(int limit = 50, int offset = 0)
        {
            var subjects = _subjectService.GetSubjects(limit, offset);
            var number = _subjectService.GetSubjectsNumber();
            return new PaginationVm<SubjectDto>(subjects, number, limit, offset);
        }

        [Route("")]
        public IHttpActionResult PostSubject(SubjectPost subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subjectDto = Mapper.Map<SubjectDto>(subject);
            subjectDto.UrlName = PrepareUrlName(subject.Name);
            var addedSubject = _subjectService.AddSubject(subjectDto, User.Identity.GetUserId());
            return Ok(addedSubject);
        }
        [Route("")]
        public IHttpActionResult PutSubject(SubjectPut subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subjectDto = Mapper.Map<SubjectDto>(subject);
            subjectDto.UrlName = PrepareUrlName(subject.Name);
            var updatedSubject = _subjectService.UpdateSubject(subjectDto, User.Identity.GetUserId());
            return Ok(updatedSubject);
        }

        [Route("{subjectId:int}")]
        public IHttpActionResult DeleteSubject(int subjectId)
        {
            _subjectService.DeleteSubject(subjectId);
            return Ok();
        }

        [Limit(50), Offset]
        [Route("{subjectId:int}/news")]
        public PaginationVm<NewsDto> GetNews(int subjectId, int limit = 50, int offset = 0)
        {
            var news = _subjectService.GetNews(subjectId);
            var number = _subjectService.GetNewsNumber(subjectId);
            return new PaginationVm<NewsDto>(news, number, limit, offset);
        }

        [Route("{subjectId:int}/news")]
        public IHttpActionResult PostNews(int subjectId, [FromBody]NewsPost news)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var newsDto = Mapper.Map<NewsDto>(news);
            var addedNews = _subjectService.AddNews(subjectId, newsDto, User.Identity.GetUserId());
            return Ok(addedNews);
        }

        [Route("{subjectId:int}/news/{newsId:int}")]
        public IHttpActionResult PutNews(int subjectId, int newsId, [FromBody]NewsPut news)
        {
            if (newsId != news.Id)
                return BadRequest("Ids are not the same");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var newsDto = Mapper.Map<NewsDto>(news);
            var updatedNews = _subjectService.UpdateNews(subjectId, newsDto);
            return Ok(updatedNews);
        }


        [Route("{subjectId:int}/news/{newsId:int}")]
        public IHttpActionResult DeleteNews(int subjectId, int newsId)
        {
            _subjectService.DeleteNews(subjectId, newsId);
            return Ok();
        }

        [Route("{subjectId:int}/teachers")]
        public IEnumerable<UserTeachingVm> GetTeachers(int subjectId)
        {
            var teachers = _subjectService.GetTeachers(subjectId)
                .Select(s => new UserTeachingVm
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email
                });
            return teachers;
        }

        [HttpPost]
        [Route("{subjectId:int}/teachers")]
        public IHttpActionResult AddTeachers(int subjectId, [FromBody]string[] teacherIds)
        {
            _subjectService.AddTeachers(subjectId, teacherIds.Distinct());
            return Ok();
        }

        [HttpDelete]
        [Route("{subjectId:int}/teachers")]
        public IHttpActionResult DeleteTeachers(int subjectId, [FromBody]string[] teacherIds)
        {
            _subjectService.DeleteTeachers(subjectId, teacherIds.Distinct());
            return Ok();
        }

        [Limit(50), Offset]
        [Route("{subjectId:int}/students")]
        public PaginationVm<UserTeachingVm> GetStudents(int subjectId, int limit = 50, int offset = 0)
        {
            var students = _subjectService.GetStudents(subjectId, limit, offset)
                .Select(s => new UserTeachingVm
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email
                });
            var count = _subjectService.GetStudentsNumber(subjectId);
            return new PaginationVm<UserTeachingVm>(students, count, limit, offset);
        }

        private string PrepareUrlName(string name)
        {
            return HttpUtility.UrlEncode(name.Substring(0, 32 > name.Length ? name.Length : 32));
        }
    }
}