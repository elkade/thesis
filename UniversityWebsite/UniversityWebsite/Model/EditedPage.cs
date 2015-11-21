using System;
using System.Collections.Generic;

namespace UniversityWebsite.Model
{
    public class EditedPage
    {
        public string Title;
        public string UrlName;
        public string Content;
        public int LangGroup;
        public DateTime CreationDate;
        public DateTime LastUpdateDate;
        public string CountryCode;
        public string ParentUrlName;
        List<string> NotUsedLanguages = new List<string>(); 
    }
}