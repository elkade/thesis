using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.UnitTests.MenuTests
{
    public partial class MenuServiceTests
    {
        [Test]
        public void Exists_ReturnsTrue_IfExists()
        {
            bool result = _languageService.Exists("pl");
            Assert.IsTrue(result);
        }
    }
}
