using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using UniversityWebsite.Core;

namespace UniversityWebsite.UnitTests
{
    public static class ExtensionMethods
    {
        public static Mock<IDomainContext> SetupDbSet<T>(this Mock<IDomainContext> contextMock, ICollection<T> data, Expression<Func<IDomainContext, IDbSet<T>>> propExp) where T : class
        {
            var queryableData = data.AsQueryable();
            var dbSetMock = new Mock<IDbSet<T>>();
            dbSetMock.Setup(m => m.Provider).Returns(queryableData.Provider);
            dbSetMock.Setup(m => m.Expression).Returns(queryableData.Expression);
            dbSetMock.Setup(m => m.ElementType).Returns(queryableData.ElementType);
            dbSetMock.Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            dbSetMock.Setup(m => m.Add(It.IsAny<T>())).Returns((T t) =>
            {
                data.Add(t);
                return t;
            });
            contextMock
                .Setup(propExp)
                .Returns(() => dbSetMock.Object);
            return contextMock;
        }
    }
}
