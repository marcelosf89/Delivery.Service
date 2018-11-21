using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Domain.Model;
using System.Collections.Generic;

namespace DeliveryService.Application.Query.Delivery.BestRouteBridge
{
    public interface IBestRouteImplementor
    {
        GetRouteByPointOriginAndDestinationQueryResponse Get(IEnumerable<Route> grafo, string origin, string destination);
    }
}
