using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.UnitTests.LanguageTests
{
    public partial class LanguageServiceTests
    {
        [Test]
        public void Exists_ReturnsTrue_IfExists()
        {
            bool result = _languageService.Exists("pl");
            Assert.IsTrue(result);
        }

        [Test]
        public void Exists_ReturnsFalse_IfDoesNotExist()
        {
            bool result = _languageService.Exists("safd");
            Assert.IsFalse(result);
        }

        [Test]
        public void Add_ThrowsPropertyValidation_OnInvalidKey()
        {
            var dictionary = new DictionaryDto
            {
                CountryCode = "sv",
                Title = "svenska",
                Words = new Dictionary<string, string>
                {
                    {"key11", "val1pl"},
                    {"key2", "val2pl"},
                    {"key3", "val3pl"},
                    {"key4", "val4pl"},
                    {"key5", "val5pl"},
                    {"key6", "val6pl"},
                }
            };
            TestDelegate dlgt = () => _languageService.AddLanguage(dictionary);
            Assert.Throws<PropertyValidationException>(dlgt);

        }

        [Test]
        public void Add_ThrowsPropertyValidation_OnExistingCountryCode()
        {
            var dictionary = new DictionaryDto
            {
                CountryCode = "pl",
                Title = "polski",
                Words = new Dictionary<string, string>
                {
                    {"key1", "val1pl"},
                    {"key2", "val2pl"},
                    {"key3", "val3pl"},
                    {"key4", "val4pl"},
                    {"key5", "val5pl"},
                    {"key6", "val6pl"},
                }
            };
            TestDelegate dlgt = () => _languageService.AddLanguage(dictionary);
            Assert.Throws<PropertyValidationException>(dlgt);
        }

        [Test]
        public void Add_AddsLanguage_TilesMenu_MainMenu()
        {
            var dictionary = new DictionaryDto
            {
                CountryCode = "en",
                Title = "english",
                Words = new Dictionary<string, string>
                {
                    {"key1", "val1pl"},
                    {"key2", "val2pl"},
                    {"key3", "val3pl"},
                    {"key4", "val4pl"},
                    {"key5", "val5pl"},
                }
            };
            _languageService.AddLanguage(dictionary);

            Assert.IsNotNull(_languages.SingleOrDefault(l => l.CountryCode == "en"));

            Assert.IsNotNull(_menus.SingleOrDefault(m => m.Group.Id == 1 && m.Language.CountryCode == "en"));
            Assert.IsNotNull(_menus.SingleOrDefault(m => m.Group.Id == 2 && m.Language.CountryCode == "en"));
        }
    }
}
