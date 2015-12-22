using NUnit.Framework;

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
    }
}
