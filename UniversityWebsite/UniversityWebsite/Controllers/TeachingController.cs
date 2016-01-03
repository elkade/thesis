using System.Linq;
using System.Net.Configuration;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Filters;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Model.Page;
using System.Collections.Generic;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za wyświetlanie danych modułu dydaktyki systemu.
    /// </summary>
    [MainMenu]
    public class TeachingController : Controller
    {
        private readonly ISubjectService _subjectService;
        private readonly IPageService _pageService;
        private readonly ApplicationUserManager _userManager;

        /// <summary>
        /// Tworzy nową instancję kontrolera.
        /// </summary>
        /// <param name="subjectService">Serwis odpowiedzialny za zarządzanie przedmiotami systemu</param>
        /// <param name="pageService">Serwis odpowiedzialny za zarządzanie stronami systemu</param>
        /// <param name="userManager">Manager odpowiedzialny za bezpośrednie zarządzanie użytkownikami</param>
        public TeachingController(ISubjectService subjectService, IPageService pageService, ApplicationUserManager userManager)
        {
            _subjectService = subjectService;
            _pageService = pageService;
            _userManager = userManager;
        }

        /// <summary>
        /// Zwraca widok listy semestrów.
        /// </summary>
        /// <returns>Obiekt widoku listy semestrów</returns>
        public ActionResult Index()
        {
            return View(new TeachingVm { SemestersCount = 10, NavMenu = new NavMenuVm { Items = GetSiblings(), IsTopLevel = true } });
        }

        /// <summary>
        /// Zwraca widok semestru zawierający listę przypianych do niego przedmiotów.
        /// </summary>
        /// <param name="number">Numer semestru</param>
        /// <returns>Obiekt widoku</returns>
        public ActionResult Semester(int number)
        {
            var userId = User.Identity.GetUserId();
            var subjects = _subjectService.GetSubjectsBySemester(number, 100, 0);

            bool isStudent = _userManager.IsInRole(userId, Consts.StudentRole);

            return View(new SemesterVm
            {
                Subjects = subjects.Select(s => new SubjectListElementVm
                {
                    SubjectName = s.Name,
                    SubjectUrlName = s.UrlName,
                    SignUpAction = isStudent ? _subjectService.GetAvailableAction(userId, s.Id) : SignUpAction.None,
                    SubjectId = s.Id
                }).ToList(),
                NavMenu = new NavMenuVm{Items = GetSiblings(), IsTopLevel = true},
                Number = number
            });
        }

        /// <summary>
        /// Zwraca widok przedmiotu.
        /// </summary>
        /// <param name="name">Nazwa przedmiotu</param>
        /// <returns>Obiekt widoku</returns>
        /// <exception cref="NotFoundException"></exception>
        public ActionResult Subject(string name)
        {
            var subject = _subjectService.GetSubject(name);

            if (subject == null)
                throw new NotFoundException("subject");

            var userId = User.Identity.GetUserId();
            var subjectVm = Mapper.Map<SubjectVm>(subject);

            if (userId == null || !subject.HasStudent(userId))
                subjectVm.Files.Clear();

            subjectVm.NavMenu = new NavMenuVm {Items = GetSiblings(), IsTopLevel = true};

            return View(subjectVm);
        }

        private List<PageMenuItemVm> GetSiblings()
        {
            var sib =  _pageService.GetParentlessPagesWithChildren((string)Session[Consts.SessionKeyLang]).ToList();
            return Mapper.Map<List<PageMenuItemVm>>(sib);
        }
    }
}