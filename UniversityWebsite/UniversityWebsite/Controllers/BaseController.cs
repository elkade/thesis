using System;
using System.Web;
using System.Web.Mvc;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            
        }
        //private readonly IMenuService _menuService;
        //private int _langId;

        //public BaseController(IMenuService menuService)
        //{
        //    _menuService = menuService;
        //}

        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    var mainMenuData = _menuService.GetMainMenu(_langId);
        //    MenuViewModel menu = new MenuViewModel(mainMenuData);
        //    ViewData["Menu"] = menu;
        //}

        public void SetCookie(string key, string value)
        {
            var encodedValue = HttpUtility.UrlEncode(value);
            var cookie = new HttpCookie(key, encodedValue)
            {
                HttpOnly = true,
            };
            Response.AppendCookie(cookie);
        }

        public string GetCookie(string key)
        {
            var cookie = Request.Cookies[key];
            if (cookie == null)
                return null;
            var decodedValue = HttpUtility.UrlDecode(cookie.Value);
            return decodedValue;
        }
    }
}