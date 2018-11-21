using BenchmarkDotNet.Attributes;
using DeliveryService.Infrastructure.Data;
using DeliveryService.Api.Controllers;
using Moq;
using DeliveryService.Application.Query.PointManager;
using DeliveryService.Application.Command.PointManager;
using DeliveryService.Crosscutting.Request.PointManagement;
using DeliveryService.Domain.Model;
using System;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Crosscutting.Exceptions;

namespace DeliveryService.Performance.Tests.Controllers
{
    [Config(typeof(ManualConfiguration))]
    public class PointsControllerTests
    {
        private Mock<IPointData> _pointDataMock;
        private IPointData _pointData;
        private Mock<IRouteData> _routeDataMock;
        private IRouteData _routeData;
        private PointsController _pointsController;

        public string _pointCode = Guid.NewGuid().ToString();

        private Point _pointBase;

        [GlobalSetup()]
        public void GlobalSetupGetPoint()
        {
            _pointDataMock = new Mock<IPointData>();
            _routeDataMock = new Mock<IRouteData>();

            _pointBase = PointsData(_pointCode);

            _pointData = _pointDataMock.Object;
            _routeData = _routeDataMock.Object;

            _pointsController = new PointsController(
                new GetPointByCodeQuery(_pointData),
                new SavePointCommand(_pointData),
                new UpdatePointCommand(_pointData),
                new DeletePointCommand(_pointData, _routeData));
        }

        [Benchmark]
        [BenchmarkCategory("PointController", "Save", "Ok")]
        public void SaveOk()
        {
            _pointsController.Save(new SavePointRequest
            {
                Code = _pointBase.Code + "new.Point",
            });
        }

        [Benchmark]
        [BenchmarkCategory("PointController", "Save", "CodeExists")]
        public void CreatePointPointEmailExists()
        {
            try
            {
                _pointsController.Save(new SavePointRequest
                {
                    Code = _pointBase.Code
                });
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("PointController", "Delete", "PointNotExists")]
        public void DeletePointNotExists()
        {
            try
            {
                _pointsController.Delete("newPoint");
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("PointController", "Delete", "Ok")]
        public void DeleteOk()
        {
            _pointsController.Delete(_pointBase.Code);
        }

        [Benchmark]
        [BenchmarkCategory("PointController", "Update", "PointNotExists")]
        public void UpdatePointNotExists()
        {
            try
            {
                _pointsController.Update(new UpdatePointRequest
                {
                    Code = "new",
                    Description = "new description"
                });
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("PointController", "Update", "Ok")]
        public void UpdateOk()
        {
            _pointsController.Update(new UpdatePointRequest
            {
                Code = _pointBase.Code,
                Description = "new description"
            });
        }

        [Benchmark]
        [BenchmarkCategory("PointController", "Get", "PointNotExists")]
        public void GetPointPointNotExists()
        {
            try
            {
                _pointsController.Get("newPoint");
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("PointController", "Get", "Ok")]
        public IActionResult GetPointOk()
        {
            return _pointsController.Get(_pointBase.Code);
        }

        public Point PointsData(string code)
        {
            Point point = new Point
            {
                Code = code
            };

            _pointDataMock.Setup(p => p.GetPointByCode(It.IsIn(point.Code)))
                .Returns(point);


            return point;
        }

    }
}
