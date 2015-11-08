using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;
using WebGrease.Css.Extensions;

namespace UniversityWebsite.Controllers
{
    public class PagesController : ApiController
    {
        private readonly PageService _pageService;

        public PagesController()
        {
            _pageService = new PageService(new DomainContext());
        }

        public IEnumerable<PageViewModel> GetAllPages()
        {
            return _pageService.GetAll().Select(Mapper.Map<PageViewModel>);
        }

        [System.Web.Http.HttpGet]
        public PageViewModel GetPage(string pageName)
        {
            var page = _pageService.FindPage(pageName);
            if (page == null)
                return new PageViewModel { Name = "NotFound" };
            var pageVm = Mapper.Map<PageViewModel>(page);
            return pageVm;
        }

        [System.Web.Http.HttpPost]
        public bool Put(PageViewModel pageVm)
        {
            if (_pageService.FindPage(pageVm.Name) == null)
                return false;
            var page = Mapper.Map<Page>(pageVm);
            _pageService.UpdateContent(page);
            return true;
        }
    }
}
