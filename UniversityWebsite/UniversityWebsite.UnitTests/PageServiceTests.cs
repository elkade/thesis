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
using UniversityWebsite.Services.Exceptions;

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
        #region FindPageById
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
        #endregion

        #region FindPageByUrlName
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
        #endregion

        #region FindTranslationByName
        [Test]
        public void FindTranslationByName_Finds()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            Data.Add(translation1);
            Data.Add(translation2);
            Data.Add(translation3);

            var result = _pageService.FindTranslation(translation1.UrlName, "de");

            Assert.AreEqual(translation3.Id, result.Id);
        }
        [Test]
        public void FindTranslationByName_CantFindTranslation()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            Data.Add(translation1);
            Data.Add(translation2);
            Data.Add(translation3);

            TestDelegate dlgt = () => _pageService.FindTranslation(translation1.UrlName, "cz");

            Assert.Throws<NotFoundException>(dlgt);
        }
        [Test]
        public void FindTranslationByName_CantFindTranslated()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            Data.Add(translation1);
            Data.Add(translation2);
            Data.Add(translation3);

            TestDelegate dlgt = () => _pageService.FindTranslation("abc4", "pl");

            Assert.Throws<NotFoundException>(dlgt);
        }
        #endregion

        #region FindTranslationById
        [Test]
        public void FindTranslationById_Finds()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            Data.Add(translation1);
            Data.Add(translation2);
            Data.Add(translation3);

            var result = _pageService.FindTranslation(translation1.Id, "de");

            Assert.AreEqual(translation3.UrlName, result.UrlName);
        }
        [Test]
        public void FindTranslationById_CantFindTranslation()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            Data.Add(translation1);
            Data.Add(translation2);
            Data.Add(translation3);

            TestDelegate dlgt = () => _pageService.FindTranslation(translation1.Id, "cz");

            Assert.Throws<NotFoundException>(dlgt);
        }
        [Test]
        public void FindTranslationById_CantFindTranslated()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            Data.Add(translation1);
            Data.Add(translation2);
            Data.Add(translation3);

            TestDelegate dlgt = () => _pageService.FindTranslation(1219, "pl");

            Assert.Throws<NotFoundException>(dlgt);
        }
        #endregion

        #region GetTranslations
        [Test]
        public void GetTranslations_Gets()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            Data.Add(translation1);
            Data.Add(translation2);
            Data.Add(translation3);

            var result = _pageService.GetTranslations(translation1.UrlName).Select(t=>t.UrlName).ToList();

            Assert.Contains(translation1.UrlName, result);
            Assert.Contains(translation2.UrlName, result);
            Assert.Contains(translation3.UrlName, result);
            Assert.AreEqual(3, result.Count);
        }
        #endregion
    }
}
