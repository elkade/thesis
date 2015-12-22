using System.Linq;
using System.Net;
using System.Reflection;
using Moq;
using NUnit.Framework;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.UnitTests.PageTests
{
    [TestFixture]
    public partial class PageServiceTests
    {
        [Test]
        public void GetsParentlessPagesWithChildren()
        {
            var pages = _pageService.GetParentlessPagesWithChildren("pl").ToArray();

            Assert.AreEqual(3, pages.Length);
            Assert.AreEqual(2, pages.Single(p=>p.Title=="a").Children.Count);
        }

        [Test]
        public void ParentlessPagesWithChildren_ReturnsEmptyList()
        {
            var pages = _pageService.GetParentlessPagesWithChildren("gr").ToArray();

            Assert.IsEmpty(pages);
        }

        [Test]
        public void GetAll_ReturnsAllPages()
        {
            var pages = _pageService.GetAll().ToArray();

            Assert.AreEqual(8,pages.Length);

            var titles = new[] {"a", "b", "c", "d", "e", "f"};

            foreach (var title in titles)
                Assert.IsTrue(pages.Any(p=>p.Title==title));
        }

        [Test]
        public void UpdateContent_UpdatesContent()
        {
            string name = "Page-b";
            string con = "Nowy kontent";

            //_domainContextMock
            //    .Setup(x => x.SetModified(It.IsAny<Page>()))
            //    .Callback((object o) =>
            //    {
            //        ((Page)o).Content = con;
            //    });

            var page = new PageDto
            {
                UrlName = name,
                Content = con
            };
            _pageService.UpdateContent(page);

            Assert.IsTrue(_pages.Any(p => p.UrlName == name && p.Content==con));
        }

        [Test]
        public void UpdateContent_ThrowsNotFound()
        {
            string name = "Page-x";
            string con = "Nowy kontent";
            var page = new PageDto
            {
                UrlName = name,
                Content = con
            };
            TestDelegate dlgt = ()=>_pageService.UpdateContent(page);

            Assert.Throws<NotFoundException>(dlgt);
        }

        [Test]
        public void AddPage_AddsPageWithUniqueUrlNameSet()
        {
            string con = "Kontent nowej strony.";
            string tit = "Tytuł nowej strony.";
            var pageToAdd = new PageDto
            {
                Content = con,
                Title = tit,
                UrlName = "abc",
                GroupId = 1,
                CountryCode = "pl"
            };
            _pageService.Add(pageToAdd);
            Assert.IsNotNull(_pages.SingleOrDefault(p=>p.UrlName=="abc" && pageToAdd.Title==tit && pageToAdd.Content==con));
        }

        [Test]
        public void AddPage_AddsPageWithNonUniqueUrlNameSet()
        {
            string con = "Kontent nowej strony.";
            string tit = "Tytuł nowej strony.";
            var pageToAdd = new PageDto
            {
                Content = con,
                Title = tit,
                UrlName = "existing",
                GroupId = 1,
                CountryCode = "pl"
            };
            _pageService.Add(pageToAdd);
            string name = _pages[_pages.Count - 1].UrlName;
            Assert.AreEqual(1, _pages.Count(p => p.UrlName == name && pageToAdd.Title == tit && pageToAdd.Content == con));
        }

        [Test]
        public void AddPage_AddsPageWithUnvalidUrlNameSet()
        {
            string con = "Kontent nowej strony.";
            string tit = "Tytuł nowej strony.";
            var pageToAdd = new PageDto
            {
                Content = con,
                Title = tit,
                UrlName = "abc def",
                GroupId = 1,
                CountryCode = "pl"
            };
            _pageService.Add(pageToAdd);
            Assert.AreEqual(1, _pages.Count(p => p.UrlName == "abc-def" && pageToAdd.Title == tit && pageToAdd.Content == con));
        }

        [Test]
        public void AddPageNonUnique_ThrowsPropertyValidationException()
        {
            var pageToAdd = new PageDto
            {
                GroupId = 2,
                CountryCode = "pl"
            };
            TestDelegate dlgt = () => _pageService.Add(pageToAdd);

            Assert.Throws<PropertyValidationException>(dlgt);
        }

        [Test]
        public void GetTranslationsLanguages_ReturnsPageList()
        {
            var pages1 = _pageService.GetTranslations("Page-b").ToArray();
            var pages2 = _pageService.GetTranslations("existing").ToArray();
            Assert.AreEqual(2, pages1.Length);
            Assert.AreEqual(1, pages2.Length);
        }

        [Test]
        public void GetTranslationsLanguages_ThrowsNotFound()
        {
            TestDelegate dlgt = () => _pageService.GetTranslations("siała baba mak");

            Assert.Throws<NotFoundException>(dlgt);

        }
    }
}
