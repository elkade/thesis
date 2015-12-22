using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.UnitTests.DictionaryTests
{
    public partial class DictionaryServiceTests
    {
        [Test]
        public void ServiceGetsTranslation()
        {
            var translation = _dictionaryService.GetTranslation("key1", "fr");

            Assert.AreEqual(translation, "val1fr");
        }

        [Test]
        public void ServiceReturnsNullWhenNoTranslation()
        {
            var translation = _dictionaryService.GetTranslation("key6", "fr");

            Assert.AreEqual(translation, null);
        }

        [Test]
        public void ServiceReturnsExactlyAllKeys()
        {
            var keys = _dictionaryService.GetKeys().ToArray();

            Assert.AreEqual(keys.Length, 5);

            for (int i = 1; i <= 5; i++)
                Assert.Contains("key" + i, keys);
        }

        [Test]
        public void GetDictionary_ReturnsDictionary()
        {
            var dict = _dictionaryService.GetDictionary("fr");

            for (int i = 1; i <= 5; i++)
                Assert.AreEqual("val" + i + "fr", dict.Words["key" + i]);
        }

        [Test]
        public void GetDictionary_ReturnsNull()
        {
            var dict = _dictionaryService.GetDictionary("de");

            Assert.IsNull(dict);
        }

        [Test]
        public void GetDictionaries_ReturnsExactlyAllDictionaries()
        {
            var dicts = _dictionaryService.GetDictionaries().ToArray();

            Assert.AreEqual(2, dicts.Length);
        }

        [Test]
        public void UpdateDictionary_ThrowsNotFoundLanguage()
        {
            var dictionary = new DictionaryDto{CountryCode = "gr", Title = "grecki", Words = new Dictionary<string, string>
                {
                    {"key1", "val1gr"},
                    {"key2", "val2gr"},
                    {"key3", "val3gr"},
                    {"key4", "val4gr"},
                }};
            Assert.Throws<NotFoundException>(() => _dictionaryService.UpdateDictionary(dictionary));
        }

        [Test]
        public void UpdateDictionary_ThrowsNotFoundKey()
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
            Assert.Throws<NotFoundException>(() => _dictionaryService.UpdateDictionary(dictionary));
        }

        [Test]
        public void UpdateDictionary_UpdatesWantedPhrases()
        {
            var dictionary = new DictionaryDto
            {
                CountryCode = "pl",
                Title = "polski",
                Words = new Dictionary<string, string>
                {
                    {"key1", "newVal1pl"},
                }
            };
            _dictionaryService.UpdateDictionary(dictionary);
            var phrase1 = _phrases.SingleOrDefault(p => p.Key == "key1" && p.CountryCode=="pl");
            Assert.IsNotNull(phrase1);
            Assert.AreEqual("newVal1pl", phrase1.Value);
        }

        [Test]
        public void UpdateDictionary_DoesNotUpdateOtherPhrases()
        {
            var dictionary = new DictionaryDto
            {
                CountryCode = "pl",
                Title = "polski",
                Words = new Dictionary<string, string>
                {
                    {"key1", "newVal1pl"},
                }
            };
            _dictionaryService.UpdateDictionary(dictionary);
            var phrase5 = _phrases.SingleOrDefault(p => p.Key == "key5" && p.CountryCode == "pl");
            Assert.IsNotNull(phrase5);
            Assert.AreEqual("val5pl", phrase5.Value);
        }

        [Test]
        public void UpdateDictionaries_UpdatesDictionaries()
        {
            var dictionaries = new List<DictionaryDto>
            {
                new DictionaryDto{CountryCode = "pl", Title = "polski", Words = new Dictionary<string, string>
                {
                    {"key1", "val1pl"},
                }},
                new DictionaryDto{CountryCode = "fr", Title = "francois", Words = new Dictionary<string, string>
                {
                    {"key4", "newVal4fr"},
                }}
            };
            _dictionaryService.UpdateDictionaries(dictionaries);

            Assert.AreEqual(10, _phrases.Count);

            Assert.IsTrue(_phrases.Any(p => p.CountryCode == "pl" && p.Key == "key1" && p.Value == "val1pl"));
            Assert.IsTrue(_phrases.Any(p => p.CountryCode == "fr" && p.Key == "key4" && p.Value == "newVal4fr"));
        }
    }
}
