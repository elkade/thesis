using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.UnitTests.SubjectTests
{
    [TestFixture]
    public partial class SubjectServiceTests
    {
        private ISubjectService _subjectService;

        private List<Subject> _subjects;

        //[OneTimeSetUp]
        public SubjectServiceTests()
        {
            AutoMapperServiceConfig.Configure();
        }

        [SetUp]
        public void SetUp()
        {
            _subjects = new List<Subject>
            {
                new Subject {Id = 1, Name = "Subject1", UrlName = "subject1", Semester = 1},
                new Subject {Id = 2, Name = "Subject2", UrlName = "subject2", Semester = 1},
                new Subject {Id = 3, Name = "Subject3", UrlName = "subject3", Semester = 2}
            };
            var contextMock = new Mock<IDomainContext>();

            contextMock
                .SetupDbSet(_subjects, x => x.Subjects);

            _subjectService = new SubjectService(contextMock.Object);

        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
