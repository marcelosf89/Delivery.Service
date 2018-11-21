using BenchmarkDotNet.Attributes;
using DeliveryService.Infrastructure.Data;
using DeliveryService.Api.Controllers;
using Moq;
using DeliveryService.Application.Query.RouteManager;
using DeliveryService.Application.Command.RouteManager;
using DeliveryService.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Crosscutting.Request.RouteManagement;

namespace DeliveryService.Performance.Tests.Controllers
{
    [Config(typeof(ManualConfiguration))]
    public class RoutesControllerTests
    {
        private Mock<IRouteData> _routeDataMock;
        private Mock<IPointData> _pointDataMock;
        private IRouteData _routeData;
        private IPointData _pointData;
        private RoutesController _routesController;

        public string _routeCode = "test.performance.01";

        private Route _routeBase;

        [GlobalSetup()]
        public void GlobalSetupGetUser()
        {
            _routeDataMock = new Mock<IRouteData>();
            _pointDataMock = new Mock<IPointData>();

            _routeBase = routesData(_routeCode);

            _routeData = _routeDataMock.Object;
            _pointData = _pointDataMock.Object;

            _routesController = new RoutesController(
                new GetRoutesByFromPointCodeQuery(_routeData),
                new SaveRouteCommand(_routeData),
                new UpdateRouteCommand(_routeData),
                new DeleteRouteCommand(_routeData));
        }

        [Benchmark]
        [BenchmarkCategory("RouteController", "Save", "Ok")]
        public void CreateRoute()
        {
            _routesController.Save(new SaveRouteRequest
            {
                PointFromCode = _routeBase + "new.route",
                PointToCode = "To"
            });
        }

        [Benchmark]
        [BenchmarkCategory("RouteController", "Save", "RouteExists")]
        public void CreateRoute_RouteExists()
        {
            try
            {
                _routesController.Save(new SaveRouteRequest
                {
                    PointFromCode = _routeBase.PointFromCode,
                    PointToCode = _routeBase.PointToCode
                });
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("RouteController", "Get", "RouteNotExists")]
        public void GetRoute_RouteNotExists()
        {
            try
            {
                _routesController.Get("test.performance.a");
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("RouteController", "Get", "Ok")]
        public IActionResult GetRoute()
        {
            return _routesController.Get(_routeBase.PointFromCode);
        }
        
        [Benchmark]
        [BenchmarkCategory("RouteController", "Update", "RouteNotExists")]
        public void UpdateRouteRouteNotExists()
        {
            try
            {
                _routesController.Update(new UpdateRouteRequest
                {
                    PointFromCode = _routeBase.PointFromCode,
                    PointToCode = _routeBase.PointToCode,
                    Cost = 2
                });
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("RouteController", "Update", "Ok")]
        public void UpdateRouteOk()
        {
            _routesController.Update(new UpdateRouteRequest
            {
                PointFromCode = _routeBase.PointFromCode,
                PointToCode = _routeBase.PointToCode,
                Cost = 2
            });
        }

        [Benchmark]
        [BenchmarkCategory("RouteController", "Delete", "RouteNotExists")]
        public void DeleteRouteRouteNotExists()
        {
            try
            {
                _routesController.Delete(new DeleteRouteRequest
                {
                    PointFromCode = _routeBase.PointFromCode,
                    PointToCode = _routeBase.PointToCode
                });
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("RouteController", "Delete", "Ok")]
        public void DeleteOk()
        {
            _routesController.Delete(new DeleteRouteRequest
            {
                PointFromCode = _routeBase.PointFromCode,
                PointToCode = _routeBase.PointToCode
            });
        }

        public Route routesData(string code)
        {
            Route route = new Route
            {
                PointFromCode = code,
                PointToCode = code + "To",
                Cost = 1,
                Time= 10
            };

            _routeDataMock.Setup(p => p.GetRoute(It.IsIn(route.PointFromCode), It.IsIn(route.PointToCode)))
                .Returns(route);

            return route;
        }

    }
}
