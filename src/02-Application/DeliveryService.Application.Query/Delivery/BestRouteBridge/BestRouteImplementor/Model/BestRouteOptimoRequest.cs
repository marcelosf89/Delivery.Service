using DeliveryService.Domain.Model;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryService.Application.Query.Delivery.BestRouteBridge.BestRouteImplementor.Model
{
    [ExcludeFromCodeCoverage]
    internal class BestRouteOptimoRequest
    {
        public BestRouteOptimoRequest(BestRouteOptimoRequest bestRote)
        {
            TotalCost = bestRote.TotalCost;
            TotalTime = bestRote.TotalTime;
            Destination = bestRote.Destination;
            Origin = bestRote.Origin;

            Routes = new LinkedList<Route>(bestRote.Routes);
        }

        public BestRouteOptimoRequest(Route route)
        {
            TotalCost = route.Cost;
            TotalTime = route.Time;
            Destination = route.PointToCode;
            Origin = route.PointFromCode;

            Routes = new LinkedList<Route>();
            Routes.AddLast(route);
        }

        public double TotalCost { get; set; }
        public double TotalTime { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public LinkedList<Route> Routes { get; set; }
    }
}
