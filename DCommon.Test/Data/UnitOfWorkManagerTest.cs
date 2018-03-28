using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.Data;
using DCommon.Data.Impl;
using Moq;
using Xunit;

namespace DCommon.Tests.Data
{
    public class UnitOfWorkManagerTest
    {
        [Fact]
        public void CurrentUnitOfWork_NotNull()
        {
            var uowMock = new Mock<IUnitOfWork>();
            var uowFactoryMock = new Mock<IUnitOfWorkFactory>();
            uowFactoryMock.Setup(m => m.Create()).Returns(uowMock.Object);
            var uowManager = new UnitOfWorkManager(uowFactoryMock.Object);
            Assert.NotNull(uowManager.CurrentUnitOfWork);
        }
    }
}
