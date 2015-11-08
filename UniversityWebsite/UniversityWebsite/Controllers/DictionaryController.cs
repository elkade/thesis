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

        public ActionResult Index(int id, string l)
        {
            if (string.IsNullOrEmpty(l))
                l = GetCookie(CookieKeyLang);
            string result = _dictionaryService.GetTranslation(id, l);
            return Content(result);
        }
	}
}