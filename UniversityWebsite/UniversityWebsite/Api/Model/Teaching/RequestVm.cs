namespace UniversityWebsite.Api.Model.Teaching
{
    public class RequestVm
    {
        public int Id { get; set; }

        public string StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentIndex { get; set; }

        public int SubjectId { get; set; }
        public string SubjectTitle { get; set; }
        public string SubjectUrlName { get; set; }

        public string Status { get; set; }
    }
}