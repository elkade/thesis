namespace UniversityWebsite.Domain.Enums
{
    /// <summary>
    /// Statuc wniosku o zapisanie na przedmiot.
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// Wniosek wysłany.
        /// </summary>
        Submitted = 0,
        /// <summary>
        /// Wniosek rozpatrzony pomyślnie.
        /// </summary>
        Approved,
        /// <summary>
        /// Wniosek odrzucony.
        /// </summary>
        Refused,
    }
}