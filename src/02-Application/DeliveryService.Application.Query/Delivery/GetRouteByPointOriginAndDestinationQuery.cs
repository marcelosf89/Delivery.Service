using DeliveryService.Application.Query.Delivery.BestRouteBridge;
using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using System.Collections.Generic;

namespace DeliveryService.Application.Query.Delivery
{
    public class GetRouteByPointOriginAndDestinationQuery : IQuery<GetRouteByPointOriginAndDestinationQueryRequest, GetRouteByPointOriginAndDestinationQueryResponse>
    {
        protected readonly IRouteData _routeData;

        public GetRouteByPointOriginAndDestinationQuery(IRouteData routeData)
        {
            _routeData = routeData;
        }

        public GetRouteByPointOriginAndDestinationQueryResponse Execute(
            GetRouteByPointOriginAndDestinationQueryRequest request)
        {
            request.PointDestinationCode = request.PointDestinationCode.ToLower();
            request.PointOriginCode = request.PointOriginCode.ToLower();

            if (request.PointOriginCode.Equals(request.PointDestinationCode))
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, "Origin and Destination have the same code");

            BestRouteRefinedAbstraction bestRoute = new BestRouteRefinedAbstraction();
            List<Route> grafo = new List<Route>();

            IEnumerable<Route> routes = _routeData.GetAllRoutes();
            grafo.AddRange(routes);

            foreach (var item in routes)
            {
                grafo.Add(new Route
                {
                    Cost = item.Cost,
                    Time = item.Time,
                    PointFromCode = item.PointToCode,
                    PointToCode = item.PointFromCode
                });
            }

            GetRouteByPointOriginAndDestinationQueryResponse result =
                bestRoute.Get(grafo, request.PointOriginCode, request.PointDestinationCode);

            if (result?.BestCost is null || result?.BestTime is null)
            {
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, "There is no route for this points");
            }

            return result;
        }
    }
}
