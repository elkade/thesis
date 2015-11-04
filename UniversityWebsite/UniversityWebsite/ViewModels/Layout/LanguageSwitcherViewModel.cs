using System.Collections.Generic;

namespace UniversityWebsite.ViewModels.Layout
{
    public class LanguageSwitcherViewModel
    {
        public List<LanguageButtonViewModel> Languages = new List<LanguageButtonViewModel>();
    }
    public class LanguageButtonViewModel
    {
        public string Title { get; set; }
        public string CountryCode { get; set; }
        public string UrlName { get; set; }
        public bool IsPage { get; set; }
    }
}