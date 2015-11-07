﻿using System.Web.Mvc;

namespace UniversityWebsite.ViewModels
{
    public class PageViewModel
    {
        public string Name { get; set; }
        public string Language { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}