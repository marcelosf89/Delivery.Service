using DeliveryService.Application.Query.Delivery.BestRouteBridge;
using DeliveryService.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace DeliveryService.Application.Query.Tests.Delivery.BestRouteBridge
{
    [TestClass]
    public class BestRouteAbstractionTest
    {
        private Mock<IBestRouteImplementor> _impMock;

        [TestInitialize]
        public void Init()
        {
            _impMock = new Mock<IBestRouteImplementor>();
        }

        [TestMethod]
        public void Get_Ok()
        {
            //Arrenge
            BestRouteAbstraction act = new BestRouteAbstraction(_impMock.Object);

            //Act
            act.Get(null, "", "");

            //Assert
            _impMock.Verify(p => p.Get(It.IsAny<IEnumerable<Route>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
