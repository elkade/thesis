using System.Collections.Generic;

namespace UniversityWebsite.Model.Menu
{
    public class LanguageMenuViewModel
    {
        public string Current { get; set; }
        public List<LanguageMenuItemViewModel> Items = new List<LanguageMenuItemViewModel>();
    }
}