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
    public class SavePointCommnadTest
    {
        private Mock<IPointData> _pointdataMock;

        [TestInitialize]
        public void Init()
        {
            _pointdataMock = new Mock<IPointData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The point already exists")]
        public void SavePointCommnad_ThrowPointCodeExists()
        {
            //Arrenge
            string code = "test";

            _pointdataMock.Setup(p => p.GetPointByCode(It.IsIn(code)))
                                .Returns(new Point
                                {
                                    Code = code,
                                    Description = "Point Test Code 2"
                                });

            //Act
            new SavePointCommand(_pointdataMock.Object).Execute(new SavePointRequest
            {
                Code = code,
                Description = "Point Test Code"
            });
        }

        [TestMethod]
        public void SavePointCommnad_Ok()
        {
            //Arrenge
            Point point = new Point
            {
                Code = "new_code",
                Description = "New Point Code",
            };

            _pointdataMock.Setup(p => p.GetPointByCode(It.IsAny<string>()));

            //Act
            new SavePointCommand(_pointdataMock.Object).Execute(new SavePointRequest
            {
                Code = point.Code,
                Description = point.Description
            });

            //Assert
            _pointdataMock.Verify(p => p.Save(It.Is((Point z) =>
                                                z.Code.Equals(point.Code) &&
                                                z.Description.Equals(point.Description) 
                                                )));
        }
    }
}
