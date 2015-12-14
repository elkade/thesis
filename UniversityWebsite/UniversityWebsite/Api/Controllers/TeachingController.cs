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
        [Authorize(Roles = "Administrator")]
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

        [Route("subjects/{subjectId:int}/news")]
        public IHttpActionResult PostNews(int subjectId, [FromBody]NewsPost news)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var newsDto = Mapper.Map<NewsDto>(news);
            var addedNews = _subjectService.AddNews(subjectId, newsDto, User.Identity.GetUserId());
            return Ok(addedNews);
        }

        [Route("subjects/{subjectId:int}/news/{newsId:int}")]
        public IHttpActionResult PutNews(int subjectId, int newsId, [FromBody]NewsPut news)
        {
            if(newsId!=news.Id)
                return BadRequest("Ids are not the same");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var newsDto = Mapper.Map<NewsDto>(news);
            var updatedNews = _subjectService.UpdateNews(subjectId, newsDto);
            return Ok(updatedNews);
        }

        [Route("subjects/{subjectId:int}/news")]
        public IEnumerable<NewsDto> GetNews(int subjectId)
        {
            return _subjectService.GetNews(subjectId);
        }


        [Route("subject/{subjectId:int}/news/{newsId:int}")]
        public IHttpActionResult DeleteNews(int subjectId, int newsId)
        {
            _subjectService.DeleteNews(subjectId, newsId);
            return Ok();
        }

        [Route("subjects/{subjectId:int}")]
        public IHttpActionResult DeleteSubject(int subjectId)
        {
            _subjectService.DeleteSubject(subjectId);
            return Ok();
        }

        private string PrepareUrlName(string name)
        {
            return HttpUtility.UrlEncode(name.Substring(0, 32 > name.Length ? name.Length : 32)); ;
        }
    }
}