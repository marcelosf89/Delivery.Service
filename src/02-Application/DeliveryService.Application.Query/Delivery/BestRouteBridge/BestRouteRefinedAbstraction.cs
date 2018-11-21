using System.Collections.Generic;
using DeliveryService.Application.Query.Delivery.BestRouteBridge.BestRouteImplementor;
using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Domain.Model;

namespace DeliveryService.Application.Query.Delivery.BestRouteBridge
{
    public class BestRouteRefinedAbstraction : BestRouteAbstraction
    {
        public BestRouteRefinedAbstraction(): this(BestRouteRefinedAbstractionType.Optimo)
        {

        }

        public BestRouteRefinedAbstraction(BestRouteRefinedAbstractionType type)
        {
            switch (type)
            {
                case BestRouteRefinedAbstractionType.Optimo:
                    implementor = new BestRouteOptimoImp();
                    break;
            }
        }

        public override GetRouteByPointOriginAndDestinationQueryResponse Get(IEnumerable<Route> grafo, string origin, string destination)
        {
            return implementor.Get(grafo, origin, destination);
        }
    }

    public enum BestRouteRefinedAbstractionType
    {
        Optimo
    }
}
