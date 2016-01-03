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
    [MainMenu]
    public class TeachingController : Controller
    {
        private readonly ISubjectService _subjectService;
        private const int PageSize = 5;
        private readonly IPageService _pageService;
        private readonly ApplicationUserManager _userManager;

        public TeachingController(ISubjectService subjectService, IPageService pageService, ApplicationUserManager userManager)
        {
            _subjectService = subjectService;
            _pageService = pageService;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View(new TeachingVm { SemestersCount = 10, NavMenu = new NavMenuVm { Items = GetSiblings(), IsTopLevel = true } });
        }

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

            if (userId == null || !subject.HasStudent(userId))
                subjectVm.Files.Clear();

            subjectVm.NavMenu = new NavMenuVm {Items = GetSiblings(), IsTopLevel = true};

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
            var sib =  _pageService.GetParentlessPagesWithChildren((string)Session[Consts.SessionKeyLang]).ToList();
            return Mapper.Map<List<PageMenuItemVm>>(sib);
        }
    }
}