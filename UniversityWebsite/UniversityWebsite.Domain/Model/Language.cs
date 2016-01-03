using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje język zdefiniowany w systemie.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Kod języka, będący równocześnie id języka w bazie danych.
        /// </summary>
        [Key]
        public string CountryCode { get; set; }
        /// <summary>
        /// Tytuł języka.
        /// </summary>
        public string Title { get; set; }
    }
}
