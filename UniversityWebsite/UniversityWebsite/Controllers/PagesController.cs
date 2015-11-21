//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Http;
//using AutoMapper;
//using UniversityWebsite.Core;
//using UniversityWebsite.Domain.Model;
//using UniversityWebsite.Helper;
//using UniversityWebsite.Services;
//using UniversityWebsite.ViewModels;

//namespace UniversityWebsite.Controllers
//{
//    public class PagesController : ApiController
//    {
//        private readonly PageService _pageService;

//        public PagesController()
//        {
//            _pageService = new PageService(new DomainContext());
//        }

//        [AntiForgeryValidate]
//        public IEnumerable<PageViewModel> GetAllPages()
//        {
//            return _pageService.GetAll().Select(page =>
//            {
//                var vm = Mapper.Map<PageViewModel>(page);
//                vm.ParentName = page.Parent != null ? page.Parent.Title : null;
//                return vm;
//            });
//        }

//        [HttpGet]
//        [AntiForgeryValidate]
//        public PageViewModel GetPage(string pageName)
//        {
//            var page = _pageService.FindPage(pageName);
//            if (page == null)
//                return new PageViewModel { Name = "NotFound" };
//            var pageVm = Mapper.Map<PageViewModel>(page);
//            return pageVm;
//        }

//        [HttpPost]
//        public bool Put(PageViewModel pageVm)
//        {
//            if (_pageService.FindPage(pageVm.Name) == null)
//                return false;
//            var page = Mapper.Map<Page>(pageVm);
//            _pageService.UpdateContent(page);
//            return true;
//        }
//    }
//}
