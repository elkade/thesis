using System.Web.Mvc;
using UniversityWebsite.Services;

namespace UniversityWebsite
{
    public class ExtendedViewPage<TModel> : WebViewPage<TModel>
    {
        private IDictionaryService _dictionaryService;
        public override void InitHelpers()
        {
            base.InitHelpers();
            _dictionaryService = DependencyResolver.Current.GetService<IDictionaryService>();
        }
        public string GetPhrase(string name)
        {
            string countryCode = (string)Session[Consts.SessionKeyLang];
            var phrase = _dictionaryService.GetTranslationCached(name, countryCode);
            if (phrase == null && countryCode != Consts.DefaultLanguage)
                phrase = _dictionaryService.GetTranslationCached(name, Consts.DefaultLanguage);
            return phrase;
        }

        public override void Execute()
        {
        }
    }
}