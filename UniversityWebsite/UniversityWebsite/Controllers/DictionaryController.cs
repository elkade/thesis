//using System.Web.Mvc;
//using UniversityWebsite.Services;

//namespace UniversityWebsite.Controllers
//{
//    public class DictionaryController : BaseController
//    {
//        private readonly IDictionaryService _dictionaryService;

//        public DictionaryController(IDictionaryService dictionaryService)
//        {
//            _dictionaryService = dictionaryService;
//        }

//        public ActionResult Index(string key, string countryCode)
//        {
//            if (string.IsNullOrEmpty(countryCode))
//                countryCode = GetCookie(CookieKeyLang);
//            string result = _dictionaryService.GetTranslation(key, countryCode);
//            return Content(result);
//        }
//    }
//}