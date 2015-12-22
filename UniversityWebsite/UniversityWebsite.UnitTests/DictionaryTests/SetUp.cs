using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.UnitTests.DictionaryTests
{
    [TestFixture]
    public partial class DictionaryServiceTests
    {
        private IDictionaryService _dictionaryService;

        private List<Phrase> _data;

        [OneTimeSetUp]
        public void Init()
        {
            AutoMapperServiceConfig.Configure();
        }

        [SetUp]
        public void SetUp()
        {
            _data = new List<Phrase>
            {
                new Phrase{CountryCode = "pl", Key = "key1", Value = "val1pl"},
                new Phrase{CountryCode = "pl", Key = "key2", Value = "val2pl"},
                new Phrase{CountryCode = "pl", Key = "key3", Value = "val3pl"},
                new Phrase{CountryCode = "pl", Key = "key4", Value = "val4pl"},
                new Phrase{CountryCode = "pl", Key = "key5", Value = "val5pl"},

                new Phrase{CountryCode = "fr", Key = "key1", Value = "val1fr"},
                new Phrase{CountryCode = "fr", Key = "key2", Value = "val2fr"},
                new Phrase{CountryCode = "fr", Key = "key3", Value = "val3fr"},
                new Phrase{CountryCode = "fr", Key = "key4", Value = "val4fr"},
                new Phrase{CountryCode = "fr", Key = "key5", Value = "val5fr"},
            };

            var queryableData = _data.AsQueryable();
            var dbSetMock = new Mock<IDbSet<Phrase>>();
            dbSetMock.Setup(m => m.Provider).Returns(queryableData.Provider);
            dbSetMock.Setup(m => m.Expression).Returns(queryableData.Expression);
            dbSetMock.Setup(m => m.ElementType).Returns(queryableData.ElementType);
            dbSetMock.Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            var domainContextMock = new Mock<IDomainContext>();
            domainContextMock
                .Setup(x => x.Phrases)
                .Returns(() => dbSetMock.Object);

            _dictionaryService = new DictionaryService(domainContextMock.Object);

        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
