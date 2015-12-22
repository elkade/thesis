using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.UnitTests.PageTests
{
    [TestFixture]
    public partial class PageServiceTests
    {
        //[OneTimeSetUp]
        public PageServiceTests()
        {
            AutoMapperServiceConfig.Configure();
        }
        private IPageService _pageService;

        List<Page> _pages;
        List<PageGroup> _groups;
        List<Language> _languages;

        private readonly Mock<IDomainContext> _domainContextMock = new Mock<IDomainContext>();

        [SetUp]
        public void SetUp()
        {
            _pages = new List<Page> 
            { 
                new Page { Title="a", Content = "", CountryCode = "pl", Id=4, GroupId = 1},
                new Page { Title="b", Content = "", CountryCode = "en", Id=6, UrlName = "Page-b", GroupId = 1},
                new Page { Title="c", Content = "", CountryCode = "pl", Id=8},
                new Page { Title="d", Content = "", CountryCode = "pl", Id=18, ParentId = 4},
                new Page { Title="e", Content = "", CountryCode = "pl", Id=28, ParentId = 4},
                new Page { Title="f", Content = "", CountryCode = "pl", Id=38, ParentId = 28},
                new Page { Title="g", Content = "", CountryCode = "pl", Id=38, ParentId = 28, UrlName = "existing", GroupId = 3},
                new Page { CountryCode = "pl", Id=38, GroupId=2},
            };
            _languages = new List<Language>
            {
                 new Language{CountryCode = "pl", Title = "polski"},
                 new Language{CountryCode = "fr", Title = "francois"}
            };
            _groups = new List<PageGroup>
            {
                 new PageGroup{Id = 1},
                 new PageGroup{Id = 2}
            };


            _domainContextMock
                .SetupDbSet(_pages, x => x.Pages)
                .SetupDbSet(_languages, x => x.Languages)
                .SetupDbSet(_groups, x => x.PageGroups);

            _pageService = new PageService(_domainContextMock.Object);

        }
        [TearDown]
        public void TearDown()
        {
            _pages = new List<Page>();
        }
        #region FindPageById
        [Test]
        public void FindPageById_Finds()
        {
            var expected = new Page { UrlName="abc", Content = "", CountryCode = "pl", Id = 5 };
            _pages.Add(expected);

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
            _pages.Add(expected);

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
            _pages.Add(translation1);
            _pages.Add(translation2);
            _pages.Add(translation3);

            var result = _pageService.FindTranslation(translation1.UrlName, "de");

            Assert.AreEqual(translation3.Id, result.Id);
        }
        [Test]
        public void FindTranslationByName_CantFindTranslation()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            _pages.Add(translation1);
            _pages.Add(translation2);
            _pages.Add(translation3);

            TestDelegate dlgt = () => _pageService.FindTranslation(translation1.UrlName, "cz");

            Assert.Throws<NotFoundException>(dlgt);
        }
        [Test]
        public void FindTranslationByName_CantFindTranslated()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            _pages.Add(translation1);
            _pages.Add(translation2);
            _pages.Add(translation3);

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
            _pages.Add(translation1);
            _pages.Add(translation2);
            _pages.Add(translation3);

            var result = _pageService.FindTranslation(translation1.Id, "de");

            Assert.AreEqual(translation3.UrlName, result.UrlName);
        }
        [Test]
        public void FindTranslationById_CantFindTranslation()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            _pages.Add(translation1);
            _pages.Add(translation2);
            _pages.Add(translation3);

            TestDelegate dlgt = () => _pageService.FindTranslation(translation1.Id, "cz");

            Assert.Throws<NotFoundException>(dlgt);
        }
        [Test]
        public void FindTranslationById_CantFindTranslated()
        {
            var translation1 = new Page { UrlName = "abc1", Content = "", CountryCode = "pl", Id = 15, GroupId = 10 };
            var translation2 = new Page { UrlName = "abc2", Content = "", CountryCode = "ru", Id = 25, GroupId = 10 };
            var translation3 = new Page { UrlName = "abc3", Content = "", CountryCode = "de", Id = 35, GroupId = 10 };
            _pages.Add(translation1);
            _pages.Add(translation2);
            _pages.Add(translation3);

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
            _pages.Add(translation1);
            _pages.Add(translation2);
            _pages.Add(translation3);

            var result = _pageService.GetTranslations(translation1.UrlName).Select(t=>t.UrlName).ToList();

            Assert.Contains(translation1.UrlName, result);
            Assert.Contains(translation2.UrlName, result);
            Assert.Contains(translation3.UrlName, result);
            Assert.AreEqual(3, result.Count);
        }
        #endregion
    }
}
