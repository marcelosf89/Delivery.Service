using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Crosscutting.Request.RouteManagement;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using System.Net;

namespace DeliveryService.Application.Command.RouteManager
{
    public class UpdateRouteCommand : ICommand<UpdateRouteRequest>
    {
        private readonly IRouteData _routeData;

        public UpdateRouteCommand(IRouteData routeData)
        {
            _routeData = routeData;
        }

        public void Execute(UpdateRouteRequest request)
        {
            if (!request.Time.HasValue && !request.Cost.HasValue)
                throw new ResponseException(HttpStatusCode.BadRequest, "Time or Cost have to be fill");

            request.PointFromCode = request.PointFromCode.ToLower();
            request.PointToCode = request.PointToCode.ToLower();

            Route route = _routeData.GetRoute(request.PointFromCode, request.PointToCode);

            if (route is null)
                throw new ResponseException(HttpStatusCode.BadRequest, "The route does not exist");

            route.Cost = GetValue(request.Cost, route.Cost);
            route.Time = GetValue(request.Time, route.Time);

            _routeData.Update(route);
        }

        private double GetValue(double? value, double oldValue)
        {
            if (!value.HasValue)
                return oldValue;

            if (value.Value <= 0)
                throw new ResponseException(HttpStatusCode.BadRequest, "The value have to be bigger than 0");

            return value.Value;
        }
    }
}
