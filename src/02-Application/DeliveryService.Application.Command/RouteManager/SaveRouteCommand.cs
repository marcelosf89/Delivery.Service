using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Crosscutting.Request.RouteManagement;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using System.Net;

namespace DeliveryService.Application.Command.RouteManager
{
    public class SaveRouteCommand : ICommand<SaveRouteRequest>
    {
        private readonly IRouteData _routeData;

        public SaveRouteCommand(IRouteData routeData)
        {
            _routeData = routeData;

        }

        public void Execute(SaveRouteRequest request)
        {
            if (request.Time <= 0)
                throw new ResponseException(HttpStatusCode.BadRequest, "It is required time bigger than 0");
            if ( request.Cost <= 0)
                throw new ResponseException(HttpStatusCode.BadRequest, "It is required cost bigger than 0");

            request.PointFromCode = request.PointFromCode.ToLower();
            request.PointToCode = request.PointToCode.ToLower();

            Route route = _routeData.GetRoute(request.PointFromCode, request.PointToCode);
            if (route != null)
                throw new ResponseException(HttpStatusCode.BadRequest, "The route already exists");

            route = new Route
            {
                PointFromCode = request.PointFromCode,
                PointToCode = request.PointToCode,
                Cost = request.Cost,
                Time = request.Time,
            };

            _routeData.Save(route);
        }
    }
}
