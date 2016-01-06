using System;
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
        private const int PageSize = 5;
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
            if (number <= 0 || number > Consts.SemestersNumber)
                throw new Exception("Semester number out of range");
            List<SubjectListElementVm> subjects = new List<SubjectListElementVm>();
            bool isStudent = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                isStudent = _userManager.IsInRole(userId, Consts.StudentRole);
                if (isStudent)
                {
                    subjects =
                        _subjectService.GetSubjectsBySemester(number, 100, 0).ToList().Select(s => new SubjectListElementVm
                        {
                            SubjectName = s.Name,
                            SubjectUrlName = s.UrlName,
                            SignUpAction = _subjectService.GetAvailableAction(userId, s.Id),
                            SubjectId = s.Id
                        }).ToList();
                }
            }
            if (!isStudent)
            {
                subjects = _subjectService.GetSubjectsBySemester(number, 100, 0).ToList().Select(s => new SubjectListElementVm
                {
                    SubjectName = s.Name,
                    SubjectUrlName = s.UrlName,
                    SignUpAction = SignUpAction.None,
                    SubjectId = s.Id
                }).ToList();
            }
            return View(new SemesterVm
            {
                Subjects = subjects,
                NavMenu = new NavMenuVm { Items = GetSiblings(), IsTopLevel = true },
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
            subjectVm.PaginateNews = new PagedData<NewsVm>
            {
                CurrentPage = 1,
                Data = subject.News.OrderByDescending(n => n.PublishDate).Take(PageSize).Select(Mapper.Map<NewsVm>).ToList(),
                NumberOfPages = Convert.ToInt32(Math.Ceiling((double)subject.News.Count() / PageSize))
            };

            if (userId == null || !(subject.HasStudent(userId) || subject.HasTeacher(userId)))
                subjectVm.Files.Clear();

            subjectVm.NavMenu = new NavMenuVm { Items = GetSiblings(), IsTopLevel = true };

            return View(subjectVm);
        }

        public ActionResult NewsList(string subjectName, int page)
        {
            var subject = _subjectService.GetSubject(subjectName);
            var news = new PagedData<NewsVm>
            {
                CurrentPage = page,
                Data = subject.News.OrderByDescending(n => n.PublishDate).Skip(PageSize * (page - 1)).Take(PageSize).Select(Mapper.Map<NewsVm>).ToList(),
                NumberOfPages = Convert.ToInt32(Math.Ceiling((double)subject.News.Count() / PageSize))
            };
            return PartialView("Sections/_News", news);
        }

        private List<PageMenuItemVm> GetSiblings()
        {
            var sib = _pageService.GetParentlessPagesWithChildren((string)Session[Consts.SessionKeyLang]).ToList();
            return Mapper.Map<List<PageMenuItemVm>>(sib);
        }
    }
}