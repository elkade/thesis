using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UniversityWebsite.Domain.Enums;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje przedmiot w systemie.
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Nazwa przedmiotu.
        /// </summary>
        [Required, StringLength(64)]
        public string Name { get; set; }
        /// <summary>
        /// Nazwa przedmitu w adresie URL.
        /// </summary>
        [Required, StringLength(96)]
        public string UrlName { get; set; }

        /// <summary>
        /// Kolekcja wpisów w sekcji "Aktualności"
        /// </summary>
        public virtual ICollection<News> News { get; set; }

        /// <summary>
        /// Sekcja "Sylabus"
        /// </summary>
        public virtual Syllabus Syllabus { get; set; }

        /// <summary>
        /// Sekcja "Plan zajęć"
        /// </summary>
        public virtual Schedule Schedule { get; set; }

        /// <summary>
        /// Kolekcja informacji o materiałach dydaktycznych należących do przedmiotu.
        /// </summary>
        public virtual ICollection<File> Files { get; set; }
        /// <summary>
        /// Kolekcja nauczycieli - administratorów przedmiotu.
        /// </summary>
        public virtual ICollection<TeacherSubject> Teachers { get; set; }
        /// <summary>
        /// Kolekcja wniosków uczniów o zapisanie na przedmiot.
        /// </summary>
        public virtual ICollection<SignUpRequest> SignUpRequests { get; set; }

        /// <summary>
        /// Numer semestru, do którego należy przedmiot.
        /// </summary>
        [Required]
        public int Semester { get; set; }

        /// <summary>
        /// Sprawdza, czy użytkownik jest studentem posiadającym dosęp do przedmiotu.
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Wartość logiczna</returns>
        public bool HasStudent(string userId)
        {
            return SignUpRequests.Any(r => r.StudentId == userId && r.Status == RequestStatus.Approved);
        }

        /// <summary>
        /// Sprawdza, czy użytkownik jest nauczycielem będącym administratrem przedmiotu.
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Wartość logiczna.</returns>
        public bool HasTeacher(string userId)
        {
            return Teachers.Any(t=>t.TeacherId == userId);
        }
    }
}