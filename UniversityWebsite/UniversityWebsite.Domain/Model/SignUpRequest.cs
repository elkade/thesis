using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityWebsite.Domain.Enums;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezemtuje wniosek o zapisanie na przedmiot.
    /// </summary>
    public class SignUpRequest
    {
        /// <summary>
        /// Tworzy nową instancję wniosku.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu, do któego odnosi się wniosek.</param>
        /// <param name="studentId">Id studenta - autora wniosku.</param>
        public SignUpRequest(int subjectId, string studentId)
        {
            SubjectId = subjectId;
            StudentId = studentId;
            Status = RequestStatus.Submitted;
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// Tworzy nową instancję wniosku.
        /// </summary>
        public SignUpRequest()
        {
            CreateTime = DateTime.Now;
        }
        /// <summary>
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Student wniskodawca.
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual User Student { get; set; }
        /// <summary>
        /// Id studenta wnioskodawcy.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Przedmiot wniosku.
        /// </summary>
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        /// <summary>
        /// Id przedmiotu wniosku.
        /// </summary>
        public int SubjectId { get; set; }

        /// <summary>
        /// Aktualny status wniosku.
        /// </summary>
        [Required]
        public RequestStatus Status { get; private set; }

        /// <summary>
        /// Data utworzenia wniosku.
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Zmienia status wniosku na "Rozpatrzony pomyślnie".
        /// </summary>
        public void Approve()
        {
            Status = RequestStatus.Approved;
        }
        /// <summary>
        /// Zmienia status wniosku na "Odrzucony".
        /// </summary>
        public void Refuse()
        {
            Status = RequestStatus.Refused;
        }

    }
}