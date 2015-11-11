namespace UniversityWebsite.Domain.Model
{
    public class SignUpRequest
    {
        public SignUpRequest()
        {
            Status = RequestStatus.Submitted;
        }

        public int Id { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }
        public RequestStatus Status { get; set; }

        public void Approve()
        {
            Status = RequestStatus.Approved;
            Subject.Students.Add(Student);
        }

    }

    public enum RequestStatus
    {
        Submitted = 0,
        Approved,
    }
}