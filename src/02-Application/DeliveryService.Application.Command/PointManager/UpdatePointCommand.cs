using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Crosscutting.Request.PointManagement;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using System.Net;

namespace DeliveryService.Application.Command.PointManager
{
    public class UpdatePointCommand : ICommand<UpdatePointRequest>
    {
        private readonly IPointData _pointdata;

        public UpdatePointCommand(IPointData pointData)
        {
            _pointdata = pointData;
        }

        public void Execute(UpdatePointRequest request)
        {
            request.Code = request.Code.ToLower();

            Point point = _pointdata.GetPointByCode(request.Code);
            if (point is null)
                throw new ResponseException(HttpStatusCode.BadRequest, "The point doesdoes not exist");

            point = new Point
            {
                Code = request.Code,
                Description = request.Description
            };

            _pointdata.Update(point);
        }
    }
}