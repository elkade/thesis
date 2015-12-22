using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

        private List<Phrase> _phrases;
        private List<Language> _languages;

        //[OneTimeSetUp]
        public DictionaryServiceTests()
        {
            AutoMapperServiceConfig.Configure();
        }

        [SetUp]
        public void SetUp()
        {
            _phrases = new List<Phrase>
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

            _languages = new List<Language>
            {
                 new Language{CountryCode = "pl", Title = "polski"},
                 new Language{CountryCode = "fr", Title = "francois"}
            };

            var contextMock = new Mock<IDomainContext>();

            contextMock
                .SetupDbSet(_phrases, x => x.Phrases)
                .SetupDbSet(_languages, x => x.Languages);

            _dictionaryService = new DictionaryService(contextMock.Object);

        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
