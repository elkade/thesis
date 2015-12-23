using System.Linq;
using NUnit.Framework;

namespace UniversityWebsite.UnitTests.SubjectTests
{
    public partial class SubjectServiceTests
    {
        [Test]
        public void GetBySemester_ReturnsList()
        {
            var subjects = _subjectService.GetSubjectsBySemester(1,30,0).ToArray();

            Assert.AreEqual(2, subjects.Length);

            Assert.Contains(_subjects[0], subjects);
            Assert.Contains(_subjects[1], subjects);
        }

        [Test]
        public void GetBySemester_ReturnsEmptyList_OnNonExistingSemester()
        {
            var subjects = _subjectService.GetSubjectsBySemester(76,30,0).ToArray();

            Assert.IsEmpty(subjects);
        }

        [Test]
        public void GetNumberBySemester_ReturnsRightNumber()
        {
            var num = _subjectService.GetSubjectsNumberBySemestser(1);

            Assert.AreEqual(2, num);
        }

        [Test]
        public void GetNumberBySemester_ReturnsZero_OnNonExistingSemester()
        {
            var num = _subjectService.GetSubjectsNumberBySemestser(154);

            Assert.AreEqual(0,num);
        }

        [Test]
        public void GetSubject_ReturnsSubject_OnExisting()
        {
            string name = "Subject1";
            string urlName = "subject1";
            var subject = _subjectService.GetSubject(urlName);

            Assert.IsTrue(subject.UrlName == urlName && subject.Name == name);
        }

        [Test]
        public void GetSubject_ReturnsNull_OnExistingExisting()
        {
            string urlName = "subject13265";
            var subject = _subjectService.GetSubject(urlName);

            Assert.IsNull(subject);
        }

        [Test]
        public void GetAll_ReturnsList()
        {
            var subjects = _subjectService.GetSubjects(30,0).ToArray();

            Assert.AreEqual(3, subjects.Length);

            Assert.Contains(_subjects[0], subjects);
            Assert.Contains(_subjects[1], subjects);
            Assert.Contains(_subjects[2], subjects);
        }

        [Test]
        public void GetAllNumber_ReturnsRightNumber()
        {
            var num = _subjectService.GetSubjectsNumber();

            Assert.AreEqual(3, num);
        }

    }
}
