using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.UnitTests.LanguageTests
{
    [TestFixture]
    public partial class LanguageServiceTests
    {
        private ILanguageService _languageService;

        private List<Phrase> _phrases;
        private List<Language> _languages;

        //[OneTimeSetUp]
        public LanguageServiceTests()
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
                 new Language{CountryCode = "fr", Title = "francois"},
                 new Language{CountryCode = "de", Title = "deutsch"},
                 new Language{CountryCode = "ru", Title = "ruski"}
            };

            var contextMock = new Mock<IDomainContext>();

            contextMock
                .SetupDbSet(_phrases, x => x.Phrases)
                .SetupDbSet(_languages, x => x.Languages);

            _languageService = new LanguageService(contextMock.Object);

        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
