using DeliveryService.Application.Command.PointManager;
using DeliveryService.Application.Command.PointManager.Models;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace DeliveryService.Application.Command.Tests.PointManager
{
    [TestClass]
    public class DeletePointCommandTest
    {
        private Mock<IPointData>  _pointdataMock;
        private Mock<IRouteData> _routedataMock;

        [TestInitialize]
        public void Init()
        {
            _pointdataMock = new Mock<IPointData>();
            _routedataMock = new Mock<IRouteData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The point code does not exist")]
        public void DeletePointCommand_ThrowPointNotExist()
        {
            //Arrange
            string code = "code";

            _pointdataMock.Setup(p => p.GetPointByCode(It.IsAny<string>()));

            //Act
            new DeletePointCommand(_pointdataMock.Object, _routedataMock.Object).Execute(new DeletePointCommnadRequest
            {
                Code = code
            });
        }

        [TestMethod]
        public void DeletePointCommand_Ok()
        {
            //Arrange
            string code = "code";

            _pointdataMock.Setup(p => p.GetPointByCode(It.IsIn(code)))
                .Returns( new Point
                {
                    Code = code,
                    Description = "Point Code"            
                });

            //Act
            new DeletePointCommand(_pointdataMock.Object, _routedataMock.Object).Execute(new DeletePointCommnadRequest
            {
                Code = code
            });

            //Assert
            _pointdataMock.Verify(p => p.Delete(It.IsIn(code)), Times.Once);
            _routedataMock.Verify(p => p.DeleteRouteFromCode(It.IsIn(code)), Times.Once);
            _routedataMock.Verify(p => p.DeleteRoutesWithPointCode(It.IsIn(code), It.IsAny<IEnumerable<string>>()), Times.Once);
        }
    }
}
