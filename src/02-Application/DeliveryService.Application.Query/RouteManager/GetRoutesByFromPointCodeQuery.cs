using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Application.Query.RouteManager
{
    public class GetRoutesByFromPointCodeQuery : IQuery<GetRoutesByFromPointCodeQueryRequest, IEnumerable<Route>>
    {
        private readonly IRouteData _routeData;

        public GetRoutesByFromPointCodeQuery(IRouteData routeData)
        {
            _routeData = routeData;
        }

        public IEnumerable<Route> Execute(GetRoutesByFromPointCodeQueryRequest request)
        {
            IEnumerable<Route> routes = _routeData.GetRoutesByPointCodeFrom(request.PointFromCode);

            if (routes is null || routes.Count() <= 0)
            {
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, $"The routes does not exist");
            }

            return routes;
        }
    }
}
