using DeliveryService.Application.Command.RouteManager;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Crosscutting.Request.RouteManagement;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DeliveryService.Application.Command.Tests.RouteManager
{
    [TestClass]
    public class SaveRouteCommnadTest
    {
        private Mock<IRouteData> _routedataMock;

        [TestInitialize]
        public void Init()
        {
            _routedataMock = new Mock<IRouteData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "It is required time bigger than 0")]
        public void SaveRouteCommnad_ThrowTimeRequiredBiggerThanZero()
        {
            //Arrenge
            SaveRouteRequest request = new SaveRouteRequest
            {
                Cost = 10,
                Time = 0
            };

            //Act
            new SaveRouteCommand(_routedataMock.Object).Execute(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "It is required cost bigger than 0")]
        public void SaveRouteCommnad_ThrowCostRequiredBiggerThanZero()
        {
            //Arrenge
            SaveRouteRequest request = new SaveRouteRequest
            {
                Cost = 0,
                Time = 10
            };

            //Act
            new SaveRouteCommand(_routedataMock.Object).Execute(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The route already exists")]
        public void SaveRouteCommnad_ThrowRouteAlreadyExists()
        {
            //Arrenge
            SaveRouteRequest request = new SaveRouteRequest
            {
                PointFromCode = "test_from",
                PointToCode = "teste_to",
                Cost = 1,
                Time = 10
            };

            _routedataMock.Setup(p => p.GetRoute(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new Route());

            //Act
            new SaveRouteCommand(_routedataMock.Object).Execute(request);
        }

        [TestMethod]
        public void SaveRouteCommnad_Ok()
        {
            //Arrenge
            SaveRouteRequest request = new SaveRouteRequest
            {
                PointFromCode = "test_from",
                PointToCode = "teste_to",
                Cost = 1,
                Time = 10
            };

            _routedataMock.Setup(p => p.GetRoute(It.IsIn(request.PointFromCode), It.IsIn(request.PointToCode)));

            //Act
            new SaveRouteCommand(_routedataMock.Object).Execute(request);

            //Assert
            _routedataMock.Verify(p => p.Save(It.Is((Route r) =>
                                                r.PointFromCode.Equals(request.PointFromCode) &&
                                                r.PointToCode.Equals(request.PointToCode) &&
                                                r.Cost== request.Cost &&
                                                r.Time == request.Time
                                                )));
        }

        [TestMethod]
        public void SaveRouteCommnad_Ok_LowerCase()
        {
            //Arrenge
            SaveRouteRequest request = new SaveRouteRequest
            {
                PointFromCode = "TEST_FROM",
                PointToCode = "TESTE_TO",
                Cost = 1,
                Time = 10
            };

            _routedataMock.Setup(p => p.GetRoute(It.IsIn(request.PointFromCode), It.IsIn(request.PointToCode)))
                .Returns(new Route());

            //Act
            new SaveRouteCommand(_routedataMock.Object).Execute(request);

            //Assert
            _routedataMock.Verify(p => p.Save(It.Is((Route r) =>
                                                r.PointFromCode.Equals(request.PointFromCode.ToLower()) &&
                                                r.PointToCode.Equals(request.PointToCode.ToLower())
                                                )));
        }
    }
}
