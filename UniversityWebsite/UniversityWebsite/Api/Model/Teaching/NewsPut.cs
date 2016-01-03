using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Teaching
{
    /// <summary>
    /// Reprezentuje dane aktualizowanego wpisu w aktualnościach
    /// </summary>
    public class NewsPut
    {
        /// <summary>
        /// Id wpisu
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Nagłówek wpisu
        /// </summary>
        [Required]
        public string Header { get; set; }
        /// <summary>
        /// Zawartość wpisu
        /// </summary>
        [Required]
        public string Content { get; set; }

    }
}