using System;
using DeliveryService.Application.Query.PointManager;
using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DeliveryService.Application.Command.Tests.PointManager
{
    [TestClass]
    public class GetPointByCodeQueryTest
    {
        private Mock<IPointData> _pointdataMock;

        [TestInitialize]
        public void Init()
        {
            _pointdataMock = new Mock<IPointData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The point does not exist")]
        public void GetPointByCodeQuery_InvalidId_ThrowResponseException400()
        {
            //Arrenge
            GetPointByCodeQueryRequest request = new GetPointByCodeQueryRequest
            {
                Code = "test"
            };

            _pointdataMock.Setup(p => p.GetPointByCode(It.IsAny<string>()));

            //Act
            new GetPointByCodeQuery(_pointdataMock.Object).Execute(request);
        }

        [TestMethod]
        public void GetPointByCodeQuery_Ok()
        {
            //Arrenge
            string code = "test";

            GetPointByCodeQueryRequest request = new GetPointByCodeQueryRequest
            {
                Code = code
            };

            _pointdataMock.Setup(p => p.GetPointByCode(It.IsIn(code)))
                .Returns(new Point
                {
                    Code = code,
                    Description= "Point Test"
                });

            //Act
            Point point = new GetPointByCodeQuery(_pointdataMock.Object).Execute(request);

            //Assert
            _pointdataMock.Verify(p => p.GetPointByCode(It.IsIn(code)), Times.Once);

            Assert.AreEqual(code, point.Code);
            Assert.AreEqual("Point Test", point.Description);
        }
    }
}
