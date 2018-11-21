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
    public class DeleteRouteCommandTest
    {
        private Mock<IRouteData> _routedataMock;

        [TestInitialize]
        public void Init()
        {
            _routedataMock = new Mock<IRouteData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The route id does not exist")]
        public void DeleteRouteCommnad_ThrowRouteNotExists()
        {
            //Arrenge
            _routedataMock.Setup(p => p.GetRoute(It.IsAny<string>(), It.IsAny<string>()));

            //Act
            new DeleteRouteCommand(_routedataMock.Object).Execute(new DeleteRouteRequest
            {
                PointFromCode = "from",
                PointToCode = "to"
            });
        }

        [TestMethod]
        public void DeleteRouteCommnad_Ok()
        {
            //Arrenge
            DeleteRouteRequest request = new DeleteRouteRequest
            {
                PointFromCode = "from",
                PointToCode = "to"
            };

            _routedataMock.Setup(p => p.GetRoute(It.IsIn(request.PointFromCode), It.IsIn(request.PointToCode)))
                .Returns(new Route()
                {
                    PointFromCode = request.PointFromCode,
                    PointToCode = request.PointToCode,
                });

            //Act
            new DeleteRouteCommand(_routedataMock.Object).Execute(request);

            //Assert
            _routedataMock.Verify(p => p.Delete(It.Is((Route r) =>
                                                r.PointFromCode.Equals(request.PointFromCode) &&
                                                r.PointToCode.Equals(request.PointToCode)
                                                )));
        }
    }
}
