using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Filters
{
    public class MainMenuAttribute : ActionFilterAttribute, IActionFilter
    {
        private readonly ILanguageService _languageService;
        private readonly IMenuService _menuService;

        public MainMenuAttribute()
        {
            _languageService = DependencyResolver.Current.GetService<ILanguageService>();
            _menuService = DependencyResolver.Current.GetService<IMenuService>();
            Order = (int)FilterScope.Controller;
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            string currentLanguage = (string)context.HttpContext.Session[Consts.SessionKeyLang];
            var mainMenu = _menuService.GetMainMenuCached(currentLanguage);
            var languages = _languageService.GetLanguagesCached();
            string url = HttpContext.Current.Request.Url.LocalPath;

            var languageMenu = new LanguageMenuViewModel();
            foreach (var language in languages)
            {
                if (language.CountryCode != currentLanguage)
                {
                    var langUrl = url + "?" + Consts.ParamKeyLang + "=" + language.CountryCode;
                    languageMenu.Items.Add(new LanguageMenuItemViewModel
                    {
                        Href = langUrl,
                        Text = language.Title,
                        CountryCode = language.CountryCode
                    });
                }
                else
                    languageMenu.Current = language.Title;
            }

            context.Controller.ViewData[Consts.MainMenuKey] = new MainMenuViewModel(mainMenu.MenuItems, languageMenu);
        }
    }
}