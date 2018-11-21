using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Crosscutting.Request.RouteManagement;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using System.Net;

namespace DeliveryService.Application.Command.RouteManager
{
    public class DeleteRouteCommand : ICommand<DeleteRouteRequest>
    {
        private readonly IRouteData _routeData;

        public DeleteRouteCommand(IRouteData routeData)
        {
            _routeData = routeData;
        }

        public void Execute(DeleteRouteRequest request)
        {
            Route route = _routeData.GetRoute(request.PointFromCode.ToLower(), request.PointToCode.ToLower());

            if (route is null)
                throw new ResponseException(HttpStatusCode.BadRequest, "The route id does not exist");

            //TODO: Send Message to verify if the points associated have others routes or they aren't routes

            _routeData.Delete(route);
        }
    }
}
