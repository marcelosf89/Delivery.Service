using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Application.Query.RouteManager;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Application.Command.Tests.RouteManager
{
    [TestClass]
    public class GetRouteByCodeQueryTest
    {
        private Mock<IRouteData> _routedataMock;

        [TestInitialize]
        public void Init()
        {
            _routedataMock = new Mock<IRouteData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The routes does not exist")]
        public void GetRouteByCodeQuery_ReturnNullRoutes_ThrowRoutesNotExist()
        {
            string code = "test";

            _routedataMock.Setup(p => p.GetRoutesByPointCodeFrom(It.IsAny<string>()));

            new GetRoutesByFromPointCodeQuery(_routedataMock.Object)
                .Execute(new GetRoutesByFromPointCodeQueryRequest
                {
                    PointFromCode = code
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The routes does not exist")]
        public void GetRouteByCodeQuery_ReturnEmptyRoutes_ThrowRoutesNotExist()
        {
            //Arrenge
            string code = "test";

            GetRoutesByFromPointCodeQueryRequest request = new GetRoutesByFromPointCodeQueryRequest
            {
                PointFromCode = code
            };

            _routedataMock.Setup(p => p.GetRoutesByPointCodeFrom(It.IsIn(code)))
                .Returns(new List<Route>());

            //Act
            new GetRoutesByFromPointCodeQuery(_routedataMock.Object)
                .Execute(request);
        }

        [TestMethod]
        public void GetRouteByCodeQuery_Ok()
        {
            //Arrenge
            string code = "test";

            GetRoutesByFromPointCodeQueryRequest request = new GetRoutesByFromPointCodeQueryRequest
            {
                PointFromCode = code
            };

            List<Route> returnMockSelect = new List<Route>()
            {
                new Route()  { PointFromCode = code },
                new Route()  { PointFromCode = code }
            };

            _routedataMock.Setup(p => p.GetRoutesByPointCodeFrom(It.IsIn(code)))
                .Returns(returnMockSelect);

            //Act
            IEnumerable<Route> routes = new GetRoutesByFromPointCodeQuery(_routedataMock.Object)
                .Execute(request);

            //Assert
            _routedataMock.Verify(p => p.GetRoutesByPointCodeFrom(It.IsIn(code)), Times.Once);

            Assert.AreEqual(2, routes.Count());
        }
    }
}
