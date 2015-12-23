﻿using System.Linq;
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
            return View(new TeachingVm { SemestersCount = 10, Siblings = GetSiblings() });
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
                Siblings = GetSiblings()
            });
        }

        public ActionResult Subject(string name)
        {
            var subject = _subjectService.GetSubject(name);

            if (subject == null)
                throw new NotFoundException("subject");

            var userId = User.Identity.GetUserId();
            var subjectVm = Mapper.Map<SubjectVm>(subject);

            if (userId == null || !subject.Students.Select(s => s.Id).Contains(userId))
                subjectVm.Files.Clear();

            subjectVm.Siblings = GetSiblings();

            return View(subjectVm);
        }

        private List<PageMenuItemVm> GetSiblings()
        {
            var sib =  _pageService.GetParentlessPagesWithChildren((string)Session[Consts.SessionKeyLang]).ToList();
            return Mapper.Map<List<PageMenuItemVm>>(sib);
        }
    }
}