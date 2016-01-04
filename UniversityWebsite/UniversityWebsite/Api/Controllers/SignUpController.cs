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
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie statusem rejestracji uczniów na przedmiot.
    /// </summary>
    [RoutePrefix("api/signup")]
    public class SignUpController : ApiController
    {
        private readonly ISubjectService _subjectService;

        /// <summary>
        /// Tworzy nową instancję kontrolera.
        /// </summary>
        /// <param name="subjectService">Serwis zarządzający przedmiotami systemu</param>
        public SignUpController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>
        /// Pobiera zbiór wniosków o dodanie do przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id danego przedmiotu</param>
        /// <param name="limit">Maksymalna liczba zwrócowych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu listy</param>
        /// <returns>Zbior obiektów reprezentujących wniosek o zapisanie się na przedmiot</returns>
        [Limit(50),Offset]
        [Route("")]
        [HttpGet]
        [ValidateWriteAccessToSubject]
        public PaginationVm<RequestVm> GetRequestsBySubject(int subjectId, int limit = 50, int offset = 0)
        {
            var requests = _subjectService.GetRequestsBySubject(subjectId, limit, offset);

            int number = _subjectService.GetRequestsNumberBySubject(subjectId);

            return new PaginationVm<RequestVm>(Mapper.Map<List<RequestVm>>(requests),number,limit,offset);
        }


        /// <summary>
        /// Pobiera zbiór wniosków o zapisanie się na przedmioty administrowane przez zalogowanego nauczyciela.
        /// </summary>
        /// <param name="limit">Maksymalna liczba zwrócowych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu listy</param>
        /// <returns>Zbiór obiektów reprezentujących wnioski o zapisanie się na przedmiot</returns>
        [Limit(50), Offset]
        [Route("")]
        [HttpGet]
        [Authorize(Roles = Consts.AdministratorRole + ", " + Consts.TeacherRole)]
        public PaginationVm<RequestVm> GetRequestsByTeacher(int limit = 50, int offset = 0)
        {
            var userId = User.Identity.GetUserId();

            var requests = _subjectService.GetRequestsByTeacher(userId, limit, offset);

            int number = _subjectService.GetRequestsNumberByTeacher(userId);

            return new PaginationVm<RequestVm>(Mapper.Map<List<RequestVm>>(requests), number, limit, offset);
        }

        /// <summary>
        /// Zmienia status wniosków na "Zatwierdzony"
        /// </summary>
        /// <param name="requestIds">Tablica id wniosków do zatwierdzenia</param>
        /// <returns>Status HTTP</returns>
        [Route("approve")]
        [HttpPost]
        [Authorize(Roles = Consts.AdministratorRole + ", " + Consts.TeacherRole)]
        public IHttpActionResult ApproveRequest(int[] requestIds)
        {
            var userId = User.Identity.GetUserId();
            _subjectService.ApproveRequests(requestIds.Distinct(), userId);
            return Ok();
        }

        /// <summary>
        /// Zmienia status wniosków na "Odrzucony"
        /// </summary>
        /// <param name="requestIds">Tablica id wniosków do odrzucenia</param>
        /// <returns>Status HTTP</returns>
        [Route("reject")]
        [HttpPost]
        [Authorize(Roles = Consts.AdministratorRole + ", " + Consts.TeacherRole)]
        public IHttpActionResult RefuseRequest(int[] requestIds)
        {
            var userId = User.Identity.GetUserId();
            _subjectService.RefuseRequests(requestIds.Distinct(), userId);
            return Ok();
        }
    }
}