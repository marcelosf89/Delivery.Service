using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Domain.Model;
using System.Collections.Generic;

namespace DeliveryService.Application.Query.Delivery.BestRouteBridge
{
    public class BestRouteAbstraction
    {
        public BestRouteAbstraction()
        {

        }

        public BestRouteAbstraction(IBestRouteImplementor implementor)
        {
            this.implementor = implementor;
        }

        protected IBestRouteImplementor implementor;

        public virtual GetRouteByPointOriginAndDestinationQueryResponse Get(IEnumerable<Route> grafo, string origin, string destination)
        {
            return implementor.Get(grafo, origin, destination);
        }
    }
}
