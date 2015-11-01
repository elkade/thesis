using System.Collections.Generic;

namespace UniversityWebsite.ViewModels.Layout
{
    public class LanguageSwitcherViewModel
    {
        public List<LanguageButtonViewModel> Languages = new List<LanguageButtonViewModel>();
    }
    public class LanguageButtonViewModel
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Page { get; set; }
        public string CountryCode { get; set; }
        public bool IsPage { get; set; }
    }
}