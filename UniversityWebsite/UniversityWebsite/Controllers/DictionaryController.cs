using System.Web.Mvc;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    public class DictionaryController : BaseController
    {
        private readonly IDictionaryService _dictionaryService;

        public DictionaryController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        public ActionResult Index(int id, string lang)
        {
            if (string.IsNullOrEmpty(lang))
                lang = GetCookie(CookieKeyLang);
            string result = _dictionaryService.GetTranslation(id, lang);
            return Content(result);
        }
	}
}