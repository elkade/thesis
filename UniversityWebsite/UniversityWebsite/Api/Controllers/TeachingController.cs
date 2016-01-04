using System;
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
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie modułem dydaktyki.
    /// </summary>
    [RoutePrefix("api/subjects")]
    public class TeachingController : ApiController
    {   
        private readonly ISubjectService _subjectService;

        /// <summary>
        /// Tworzy nową instancję kontrolera.
        /// </summary>
        /// <param name="subjectService">Serwis zarządzający przedmiotami systemu</param>
        public TeachingController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>
        /// Pobiera zbiór przedmiotów zdefiniowanych w systemie.
        /// </summary>
        /// <param name="limit">Maksymalna liczba zwrócowych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu listy</param>
        /// <returns>Zbiór obiektów reprezentujących przedmioty</returns>
        [Limit(50), Offset]
        [Route("")]
        [Authorize(Roles = "Administrator, Teacher")]
        //[AntiForgeryValidate]
        public PaginationVm<SubjectDto> GetSubjects(int limit = 50, int offset = 0)
        {
            String userId = null;
            if (User.IsInRole("Teacher"))
            {
                userId = User.Identity.GetUserId();
            }
            var subjects = _subjectService.GetSubjects(limit, offset, userId);
            var number = _subjectService.GetSubjectsNumber();
            return new PaginationVm<SubjectDto>(subjects, number, limit, offset);
        }

        /// <summary>
        /// Dodaje nowy przedmiot do systemu.
        /// </summary>
        /// <param name="subject">Dane nowego przedmiotu</param>
        /// <returns>Obiekt reprezentujący dane nowododanego przedmiotu</returns>
        [Route("")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult PostSubject(SubjectPost subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subjectDto = Mapper.Map<SubjectDto>(subject);
            subjectDto.UrlName = PrepareUrlName(subject.Name);
            var addedSubject = _subjectService.AddSubject(subjectDto, User.Identity.GetUserId());
            return Ok(addedSubject);
        }
        /// <summary>
        /// Nadpisuje dane przedmiotu istniejacego w systemie.
        /// </summary>
        /// <param name="subject">Dane, którymi ma zostać nadpisany przedmiot</param>
        /// <returns>Dane nadpisanego przedmiotu</returns>
        [Route("")]
        [Authorize(Roles = "Administrator, Teacher")]
        public IHttpActionResult PutSubject(SubjectPut subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subjectDto = Mapper.Map<SubjectDto>(subject);
            subjectDto.UrlName = PrepareUrlName(subject.Name);
            var updatedSubject = _subjectService.UpdateSubject(subjectDto, User.Identity.GetUserId());
            return Ok(updatedSubject);
        }

        /// <summary>
        /// Usuwa przedmiot z systemu.
        /// </summary>
        /// <param name="subjectId">Id usuwanego przedmiotu</param>
        /// <returns>Status HTTP</returns>
        [Route("{subjectId:int}")]
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public IHttpActionResult DeleteSubject(int subjectId)
        {
            _subjectService.DeleteSubject(subjectId);
            return Ok();
        }

        /// <summary>
        /// Pobiera listę aktualności należących do przedmoiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="limit">Maksymalna liczba zwrócowych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu listy</param>
        /// <returns>Zbiór obiektów reprezentujących aktualności</returns>
        [Limit(50), Offset]
        [Route("{subjectId:int}/news")]
        [Authorize(Roles = "Administrator, Teacher")]
        public PaginationVm<NewsDto> GetNews(int subjectId, int limit = 50, int offset = 0)
        {
            var news = _subjectService.GetNews(subjectId);
            var number = _subjectService.GetNewsNumber(subjectId);
            return new PaginationVm<NewsDto>(news, number, limit, offset);
        }

        /// <summary>
        /// Dodaje nowy wpis w aktualnościach przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="news">Dane aktualności</param>
        /// <returns>Dane dodanego wpisu</returns>
        [Route("{subjectId:int}/news")]
        [Authorize(Roles = "Administrator, Teacher")]
        public IHttpActionResult PostNews(int subjectId, [FromBody]NewsPost news)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var newsDto = Mapper.Map<NewsDto>(news);
            var addedNews = _subjectService.AddNews(subjectId, newsDto, User.Identity.GetUserId());
            return Ok(addedNews);
        }

        /// <summary>
        /// Aktualizuje dany wpis w aktualnościach danego przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="newsId">Id wpisu w aktualnościach</param>
        /// <param name="news">Dane wpisu do nadpisania</param>
        /// <returns>Dane nadpisanego wpisu</returns>
        [Route("{subjectId:int}/news/{newsId:int}")]
        [Authorize(Roles = "Administrator, Teacher")]
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


        /// <summary>
        /// Usuwa wpis w aktualnościach z przedmiotu
        /// </summary>
        /// <param name="subjectId">Id danego przedmiotu</param>
        /// <param name="newsId">Id wpisu w aktualnościach</param>
        /// <returns>Status HTTP</returns>
        [Route("{subjectId:int}/news/{newsId:int}")]
        [Authorize(Roles = "Administrator, Teacher")]
        public IHttpActionResult DeleteNews(int subjectId, int newsId)
        {
            _subjectService.DeleteNews(subjectId, newsId);
            return Ok();
        }

        /// <summary>
        /// Zwraca zbiór nauczycieli administrujących danym przedmiotem
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <returns>Zbiór obiektów reprezentujących nauczycieli</returns>
        [Route("{subjectId:int}/teachers")]
        [Authorize(Roles = "Administrator")]
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

        /// <summary>
        /// Dodaje nauczycieli do przedmiotu
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="teacherIds">Tablica id nauczycieli</param>
        /// <returns>Status HTTP</returns>
        [HttpPost]
        [Route("{subjectId:int}/teachers")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult AddTeachers(int subjectId, [FromBody]string[] teacherIds)
        {
            _subjectService.AddTeachers(subjectId, teacherIds.Distinct());
            var teachers = _subjectService.GetTeachers(subjectId)
                .Select(s => new UserTeachingVm
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email
                });
            return Ok();
        }

        /// <summary>
        /// Zabiera nauczycielowi prawa do zarządzania przedmiotem.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="teacherIds">Id nauczyciela</param>
        /// <returns>Status HTTP</returns>
        [HttpPut]
        [Route("{subjectId}/teachers")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult DeleteTeachers(int subjectId, [FromBody]string[] teacherIds)
        {
            _subjectService.DeleteTeachers(subjectId, teacherIds.Distinct());
            return Ok();
        }

        /// <summary>
        /// Pobiera listę studentów zapisanych na przedmiot
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="limit">Maksymalna liczba zwrócowych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu listy</param>
        /// <returns>Zbiór obiektów reprezentujących studentów</returns>
        [Limit(50), Offset]
        [Route("{subjectId:int}/students")]
        [Authorize(Roles = "Administrator, Teacher")]
        public PaginationVm<UserTeachingVm> GetStudents(int subjectId, int limit = 50, int offset = 0)
        {
            var students = _subjectService.GetStudents(subjectId, limit, offset)
                .Select(s => new UserTeachingVm
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    IndexNumber = s.IndexNumber
                });
            var count = _subjectService.GetStudentsNumber(subjectId);
            return new PaginationVm<UserTeachingVm>(students, count, limit, offset);
        }

        [HttpPut]
        [Route("{subjectId:int}/students")]
        [Authorize(Roles = Consts.AdministratorRole + "," + Consts.TeacherRole)]
        public IHttpActionResult RemoveStudents(int subjectId, [FromBody]string[] studentsIds)
        {
            _subjectService.RemoveFromSubject(subjectId, studentsIds);
            return Ok();
        }

        private string PrepareUrlName(string name)
        {
            return HttpUtility.UrlEncode(name.Substring(0, 32 > name.Length ? name.Length : 32));
        }
    }
}