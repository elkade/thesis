using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje tłumaczenie frazy.
    /// </summary>
    public class Phrase
    {
        /// <summary>
        /// Klucz frazy. Razem z id języka tworzy klucz główny obiektu w bazie danych.
        /// </summary>
        [Key, Column(Order = 0)]
        public string Key { get; set; }
        /// <summary>
        /// Id języka frazy. Razem z kluczem frazy tworzy klucz główny obiektu w bazie danych.
        /// </summary>
        [Key, Column(Order = 1)]
        public string CountryCode { get; set; }
        /// <summary>
        /// Wartość tłumaczenia frazy.
        /// </summary>
        [Required]
        [StringLength(512)]
        public string Value { get; set; }
    }
}
