﻿using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class Language
    {
        [Key]
        public string CountryCode { get; set; }
        public string Title { get; set; }
    }
}
