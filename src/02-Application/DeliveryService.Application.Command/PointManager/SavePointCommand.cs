using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Crosscutting.Request.PointManagement;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using System.Net;

namespace DeliveryService.Application.Command.PointManager
{
    public class SavePointCommand : ICommand<SavePointRequest>
    {
        private readonly IPointData _pointdata;

        public SavePointCommand(IPointData pointData)
        {
            _pointdata = pointData;
        }

        public void Execute(SavePointRequest request)
        {
            request.Code = request.Code.ToLower();

            Point point = _pointdata.GetPointByCode(request.Code);
            if (point != null)
                throw new ResponseException(HttpStatusCode.BadRequest, "The point already exists");

            point = new Point
            {
                Code = request.Code,
                Description = request.Description
            };

            _pointdata.Save(point);
        }
    }
}