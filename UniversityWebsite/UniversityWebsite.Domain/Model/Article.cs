using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje artykuł będący częścią przedmiotu.
    /// </summary>
    public abstract class Article
    {
        /// <summary>
        /// Id obiektu w bazie danych
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Zawartość artyjułu.
        /// </summary>
        [Column(TypeName = "text")]
        public string Content { get; set; }

        /// <summary>
        /// Id aurota artykułu.
        /// </summary>
        public string AuthorId { get; set; }
        /// <summary>
        /// Autor artykułu.
        /// </summary>
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        /// <summary>
        /// Przedmiot zawierający artykuł.
        /// </summary>
        public virtual Subject Subject { get; set; }

        /// <summary>
        /// Data publikacji artykułu.
        /// </summary>
        [Required]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Tworzy nową instancję artykułu.
        /// </summary>
        protected Article()
        {
            PublishDate = DateTime.Now;
            Content = String.Empty;
        }
    }

    /// <summary>
    /// Reprezentuje pojedynczy wpis w sekcji przedmiotu "Aktualności"
    /// </summary>
    public class News : Article
    {
        /// <summary>
        /// Nagłówek wpisu.
        /// </summary>
        [Required]
        [StringLength(64)]
        public string Header { get; set; }

        /// <summary>
        /// Id przedmiotu zawierajacego wpis.
        /// </summary>
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

    }
    /// <summary>
    /// Reprezentuje sekcję przedmiotu "Sylabus".
    /// </summary>
    public class Syllabus : Article
    {
        /// <summary>
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key, ForeignKey("Subject")]
        public new int Id { get; set; }
    }
    /// <summary>
    /// Reprezentuje sekcję przedmiotu "Plan zajęć".
    /// </summary>
    public class Schedule : Article
    {
        /// <summary>.
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key, ForeignKey("Subject")]
        public new int Id { get; set; }
    }
}
