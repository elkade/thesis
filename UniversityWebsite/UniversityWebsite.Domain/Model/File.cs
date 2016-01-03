using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje metadane pliku przechowywanego na serwerze.
    /// </summary>
    public class File 
    {
        /// <summary>
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// Nazwa pliku.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string FileName { get; set; }

        /// <summary>
        /// Data przesłania pierwszej wersji pliku na serwer.
        /// </summary>
        [Required]
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Data ostatniej aktualizacji pliku.
        /// </summary>
        [Required]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Wersja pliku.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Id użytkownika, który przesłał plik na serwer.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Użytkownik, który przesłał plik na serwer.
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User Uploader { get; set; }

        /// <summary>
        /// Przedmiot, do którego przypisany jest plik.
        /// </summary>
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

        /// <summary>
        /// Id przedmiotu, do którego przypisany jest plik.
        /// </summary>
        public int? SubjectId { get; set; }
    }
}

