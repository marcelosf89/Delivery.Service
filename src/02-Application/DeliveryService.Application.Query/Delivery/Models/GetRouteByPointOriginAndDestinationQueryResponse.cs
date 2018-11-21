using DeliveryService.Domain.Model;
using System;
using System.Collections.Generic;

namespace DeliveryService.Application.Query.PointManager.Models
{
    public class GetRouteByPointOriginAndDestinationQueryResponse
    {
        public double MinCost{ get; set; }
        public double MinTime { get; set; }
        public IEnumerable<Route> BestTime { get; set; }
        public IEnumerable<Route> BestCost { get; set; }
    }
}