using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje powiązanie nauczyciel - przedmiot w bazie danych.
    /// </summary>
    public class TeacherSubject
    {
        /// <summary>
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// NAuczyciel.
        /// </summary>
        [ForeignKey("TeacherId")]
        public virtual User Teacher { get; set; }
        /// <summary>
        /// Id nauczyciela.
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// Przedmiot.
        /// </summary>
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        /// <summary>
        /// Id przedmiotu.
        /// </summary>
        public int SubjectId { get; set; }

    }
}
