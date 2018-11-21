using System;
using System.Text;
using DeliveryService.Application.Command.PointManager;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Crosscutting.Request.PointManagement;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DeliveryService.Application.Command.Tests.PointManager
{
    [TestClass]
    public class UpdatePointCommnadTest
    {
        private Mock<IPointData> _pointdataMock;

        [TestInitialize]
        public void Init()
        {
            _pointdataMock = new Mock<IPointData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The point doesdoes not exist")]
        public void UpdatePointCommand_ThrowPointDoesntExist()
        {
            //Arrenge
            string code = "new_code";

            _pointdataMock.Setup(p => p.GetPointByCode(It.IsAny<string>()));

            //Act
            new UpdatePointCommand(_pointdataMock.Object).Execute(new UpdatePointRequest
            {
                Code = code
            });
        }

        [TestMethod]
        public void UpdatePointCommand_Ok()
        {
            //Arrenge
            Point point = new Point
            {
                Code = "code",
                Description = "New Point Code",
            };

            _pointdataMock.Setup(p => p.GetPointByCode(It.IsIn(point.Code)))
                .Returns(point);

            //Act
            new UpdatePointCommand(_pointdataMock.Object).Execute(new UpdatePointRequest
            {
                Code = point.Code,
                Description = point.Description + " new"
            });

            //Assert
            _pointdataMock.Verify(p => p.Update(It.Is((Point z) =>
                z.Code.Equals(point.Code) &&
                z.Description.Equals(point.Description + " new")
            )));
        }
    }
}
