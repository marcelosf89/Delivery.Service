using BenchmarkDotNet.Attributes;
using DeliveryService.Infrastructure.Data;
using DeliveryService.Api.Controllers;
using Moq;
using DeliveryService.Domain.Model;
using DeliveryService.Application.Query.Delivery;
using System.Collections.Generic;

namespace DeliveryService.Performance.Tests.Controllers
{
    [Config(typeof(ManualConfiguration))]
    public class DeliveryControllerTests
    {
        private Mock<IRouteData> _routeDataMock;
        private IRouteData _routeData;
        private DeliveryController _deliveryController;

        [GlobalSetup()]
        public void GlobalSetupGetPoint()
        {
            _routeDataMock = new Mock<IRouteData>();

            SetupData();

            _routeData = _routeDataMock.Object;

            _deliveryController = new DeliveryController(
                new GetRouteByPointOriginAndDestinationQuery(_routeData),
                new GetBestTimeByPointOriginAndDestinationQuery(_routeData),
                new GetBestCostByPointOriginAndDestinationQuery(_routeData));

        }

        [Benchmark]
        [BenchmarkCategory("DeliveryController", "Get", "Ok")]
        [Arguments("a", "b")]
        [Arguments("a", "i")]
        [Arguments("a", "f")]
        [Arguments("c", "i")]
        [Arguments("c", "f")]
        [Arguments("h", "b")]
        [Arguments("h", "i")]
        [Arguments("h", "f")]
        public void GetOk(string origin, string destination)
        {
            _deliveryController.Get(origin, destination);
        }

        public void SetupData()
        {
            List<Route> routes = new List<Route>()
            {
                new Route(){ PointFromCode = "a", PointToCode = "c", Cost = 20, Time= 1},
                new Route(){ PointFromCode = "a", PointToCode = "h", Cost = 1, Time= 10},
                new Route(){ PointFromCode = "a", PointToCode = "e", Cost = 5, Time= 30},
                new Route(){ PointFromCode = "h", PointToCode = "e", Cost = 1, Time= 30},
                new Route(){ PointFromCode = "e", PointToCode = "d", Cost = 5, Time= 3},
                new Route(){ PointFromCode = "d", PointToCode = "f", Cost = 50, Time= 4},
                new Route(){ PointFromCode = "f", PointToCode = "i", Cost = 50, Time= 45},
                new Route(){ PointFromCode = "f", PointToCode = "g", Cost = 50, Time= 40},
                new Route(){ PointFromCode = "c", PointToCode = "b", Cost = 12, Time= 1},
                new Route(){ PointFromCode = "i", PointToCode = "b", Cost = 5, Time= 65},
                new Route(){ PointFromCode = "g", PointToCode = "b", Cost = 73, Time= 64},
            };

            _routeDataMock.Setup(p => p.GetAllRoutes())
                .Returns(routes);

        }

    }
}
