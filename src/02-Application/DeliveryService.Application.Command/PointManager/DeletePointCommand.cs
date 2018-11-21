using DeliveryService.Application.Command.PointManager.Models;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using System.Collections.Generic;
using System.Net;

namespace DeliveryService.Application.Command.PointManager
{
    public class DeletePointCommand : ICommand<DeletePointCommnadRequest>
    {
        private readonly IPointData _pointData;
        private readonly IRouteData _routeData;

        public DeletePointCommand(IPointData pointData, IRouteData routeData)
        {
            _pointData = pointData;
            _routeData = routeData;
        }

        public void Execute(DeletePointCommnadRequest request)
        {
            Point point = _pointData.GetPointByCode(request.Code);

            if (point is null)
                throw new ResponseException(HttpStatusCode.BadRequest, $"The point code does not exist");

            IEnumerable<string> allPointsCode = _pointData.GetAllPointCode();

            _pointData.Delete(request.Code);
            _routeData.DeleteRouteFromCode(request.Code);
            _routeData.DeleteRoutesWithPointCode(request.Code, allPointsCode);
            
        }
    }
}
