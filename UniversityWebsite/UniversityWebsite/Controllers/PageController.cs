using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.Controllers
{
    [Authorize]
    public class PageController : BaseController
    {
        public PageController(IMenuService menuService, IPageService pageService, ILanguageService languageService)
            : base(menuService, pageService, languageService)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string name)
        {
            try
            {
                //todo ustawić cookie
                var page = PageService.FindPage(name);
                if (page == null)

                    Lang = page.CountryCode;
                PageId = page.Id;

                var pageVm = Mapper.Map<PageViewModel>(page);


                ViewBag.Title = page.Title;
                return View(pageVm);
            }
            catch (NotFoundException)
            {
                return null;//todo strona błędu
            }
        }

        //[HttpGet]
        //public ActionResult Edit(string name)
        //{
        //    var page = PageService.FindPage(name);
        //    if (page == null)
        //        return View(new PageViewModel { Name = "NotFound" });
        //    var pageVm = Mapper.Map<PageViewModel>(page);
        //    return View(pageVm);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(PageViewModel pageVm)
        //{
        //    if (PageService.FindPage(pageVm.Name) == null)
        //        return Json(new { success = false });
        //    var page = Mapper.Map<Page>(pageVm);
        //    PageService.UpdateContent(page);
        //    return Json(new { success = true });
        //}
        //[HttpPost]
        //public JsonResult Add(PageViewModel pageVm)
        //{
        //    if (PageService.FindPage(pageVm.Name) != null)
        //        return Json(new { success = false });
        //    var page = Mapper.Map<Page>(pageVm);
        //    PageService.Add(page);
        //    return Json(new { success = true });
        //}


        //[HttpPost]
        //public ActionResult Delete(string pageName)
        //{
        //    return Content("Delete");
        //}
    }
}