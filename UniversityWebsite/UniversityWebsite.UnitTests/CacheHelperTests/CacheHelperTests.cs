using System;
using System.Threading;
using NUnit.Framework;
using UniversityWebsite.Services.Helpers;

namespace UniversityWebsite.UnitTests.CacheHelperTests
{
    [TestFixture]
    public class CacheHelperTests
    {
        private string _key;

        [OneTimeSetUp]
        public void Init()
        {

        }

        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {
            CacheHelper.Remove(_key);
        }

        [Test]
        public void HelperCachesProperValue()
        {
            _key = "key1";
            string str = "Siała baba mak1";
            CacheHelper.Add(_key, str, TimeSpan.FromSeconds(2));
            var cachedStr = CacheHelper.Get<string>(_key);
            Thread.Sleep(1);
            Assert.AreEqual(str, cachedStr);
        }

        [Test]
        public void HelperInvokesMethod()
        {
            _key = "key2";
            string str = "Siała baba mak2";
            string str2 = "Nie wiedziała jak2";
            CacheHelper.Add(_key, str, TimeSpan.FromSeconds(2));
            var cachedStr = CacheHelper.GetOrInvoke<string>(_key, () => str2,
                TimeSpan.FromSeconds(3));
            Thread.Sleep(2);
            Assert.AreEqual(str, cachedStr);
            CacheHelper.Remove(_key);
            Thread.Sleep(2);
            cachedStr = CacheHelper.GetOrInvoke<string>(_key, () => str2,
                TimeSpan.FromSeconds(3));
            Assert.AreEqual(str2, cachedStr);
        }

        [Test]
        public void HelperRemovesProperly()
        {
            _key = "key3";
            string str = "Siała baba mak3";
            CacheHelper.Add(_key, str, TimeSpan.FromSeconds(2));
            var cachedStr = CacheHelper.Get<string>(_key);
            Thread.Sleep(1);
            Assert.AreEqual(str, cachedStr);
            CacheHelper.Remove(_key);
            cachedStr = CacheHelper.Get<string>(_key);
            Assert.AreEqual(null, cachedStr);
        }

        [Test]
        public void HelperHandlesNull()
        {
            _key = "key4";
            var cachedStr = CacheHelper.GetOrInvoke<string>(_key, () => null,
                TimeSpan.FromSeconds(3));
            Assert.AreEqual(null,cachedStr);
        }
    }
}
