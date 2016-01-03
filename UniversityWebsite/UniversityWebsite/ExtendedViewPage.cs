using System.Web.Mvc;
using UniversityWebsite.Services;

namespace UniversityWebsite
{
    /// <summary>
    /// Rozszerza standardowy obiekt widoku w frameworku, dodając metodę "GetPhrase"
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ExtendedViewPage<TModel> : WebViewPage<TModel>
    {
        private IDictionaryService _dictionaryService;
        public override void InitHelpers()
        {
            base.InitHelpers();
            _dictionaryService = DependencyResolver.Current.GetService<IDictionaryService>();
        }
        /// <summary>
        /// Pobiera tłumaczenie frazy w aktualnie wybranym języku stron aplikacji.
        /// </summary>
        /// <param name="name">Nazwa frazy</param>
        /// <returns>Tłumaczenie frazy.</returns>
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