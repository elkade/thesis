using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.UnitTests
{
    [TestFixture]
    public class PageServiceTests
    {
        [TestFixtureSetUp]
        public void Init()
        {
            AutoMapperServiceConfig.Configure();
        }
        private IPageService _pageService;

        List<Page> Data;

        [SetUp]
        public void SetUp()
        {
            Data = new List<Page> 
            { 
                new Page { Content = "", CountryCode = "pl", Id=4},
                new Page { Content = "", CountryCode = "en", Id=6},
                new Page { Content = "", CountryCode = "pl", Id=8},
            };
            var queryableData = Data.AsQueryable();
            var dbSetMock = new Mock<IDbSet<Page>>();
            dbSetMock.Setup(m => m.Provider).Returns(queryableData.Provider);
            dbSetMock.Setup(m => m.Expression).Returns(queryableData.Expression);
            dbSetMock.Setup(m => m.ElementType).Returns(queryableData.ElementType);
            dbSetMock.Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            var domainContextMock = new Mock<IDomainContext>();
            domainContextMock
                .Setup(x => x.Pages)
                .Returns(dbSetMock.Object);

            _pageService = new PageService(domainContextMock.Object);
        }
        [TearDown]
        public void TearDown()
        {
            Data = new List<Page>();
        }
        [Test]
        public void FindPageById_Finds()
        {
            var expected = new Page { UrlName="abc", Content = "", CountryCode = "pl", Id = 5 };
            Data.Add(expected);

            var result = _pageService.FindPage(expected.Id);

            Assert.AreEqual(expected.UrlName, result.UrlName);
        }
        [Test]
        public void FindPageById_ReturnsNull()
        {
            var result = _pageService.FindPage(120);

            Assert.AreEqual(null, result);
        }
        [Test]
        public void FindPageByUrlName_Finds()
        {
            var expected = new Page { UrlName = "abc", Content = "", CountryCode = "pl", Id = 5 };
            Data.Add(expected);

            var result = _pageService.FindPage(expected.UrlName);

            Assert.AreEqual(expected.Id, result.Id);
        }
        [Test]
        public void FindPageByUrlName_ReturnsNull()
        {
            var result = _pageService.FindPage("qwerty");

            Assert.AreEqual(null, result);
        }
    }
}
