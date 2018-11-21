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
    public class UpdateRouteCommnadTest
    {
        private Mock<IRouteData> _routedataMock;

        [TestInitialize]
        public void Init()
        {
            _routedataMock = new Mock<IRouteData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "Time or Cost have to be fill")]
        public void UpdateRouteCommand_ThrowRequiredTimeOrCostToBeFill()
        {
            //Arrenge
            UpdateRouteRequest request = new UpdateRouteRequest
            {
                PointFromCode = "",
                PointToCode = ""
            };

            //Act
            new UpdateRouteCommand(_routedataMock.Object).Execute(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The value have to be bigger than 0")]
        public void UpdateRouteCommand_CostZero_ThrowValueBiggerThan0()
        {
            //Arrenge
            UpdateRouteRequest request = new UpdateRouteRequest
            {
                PointFromCode = "from",
                PointToCode = "to",
                Cost = 0,
                Time = 10
            };

            _routedataMock.Setup(p => p.GetRoute(It.IsIn(request.PointFromCode), It.IsIn(request.PointToCode)))
                .Returns(new Route()
                {
                    PointFromCode = request.PointFromCode,
                    PointToCode = request.PointToCode,
                    Cost = 10,
                    Time = 10
                });

            //Act
            new UpdateRouteCommand(_routedataMock.Object).Execute(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The value have to be bigger than 0")]
        public void UpdateRouteCommand_TimeZero_ThrowValueBiggerThan0()
        {
            //Arrenge
            UpdateRouteRequest request = new UpdateRouteRequest
            {
                PointFromCode = "from",
                PointToCode = "to",
                Cost = 10,
                Time = 0
            };

            _routedataMock.Setup(p => p.GetRoute(It.IsIn(request.PointFromCode), It.IsIn(request.PointToCode)))
                .Returns(new Route()
                {
                    PointFromCode = request.PointFromCode,
                    PointToCode = request.PointToCode,
                    Cost = 10,
                    Time = 10
                });

            //Act
            new UpdateRouteCommand(_routedataMock.Object).Execute(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The route does not exist")]
        public void UpdateRouteCommand_ThrowRouteNotExist()
        {
            //Arrenge
            UpdateRouteRequest request = new UpdateRouteRequest
            {
                PointFromCode = "from",
                PointToCode = "to",
                Cost = 10,
                Time = 20
            };

            _routedataMock.Setup(p => p.GetRoute(It.IsIn(request.PointFromCode), It.IsIn(request.PointToCode)));

            //Act
            new UpdateRouteCommand(_routedataMock.Object).Execute(request);
        }

        [TestMethod]
        public void UpdateRouteCommand_Ok()
        {
            //Arrenge
            UpdateRouteRequest request = new UpdateRouteRequest
            {
                PointFromCode = "from",
                PointToCode = "to",
                Cost = 12,
                Time = 20
            };

            _routedataMock.Setup(p => p.GetRoute(It.IsIn(request.PointFromCode), It.IsIn(request.PointToCode)))
                .Returns(new Route()
                {
                    PointFromCode = request.PointFromCode,
                    PointToCode = request.PointToCode,
                    Cost = 10,
                    Time = 10
                });

            //Act
            new UpdateRouteCommand(_routedataMock.Object).Execute(request);

            //Assert
            _routedataMock.Verify(p => p.Update(It.Is((Route c) =>
                c.PointToCode == request.PointToCode &&
                c.PointFromCode == request.PointFromCode &&
                c.Cost == request.Cost &&
                c.Time == request.Time
            )));
        }
    }
}
