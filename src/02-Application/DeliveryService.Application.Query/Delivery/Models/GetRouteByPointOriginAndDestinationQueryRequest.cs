using System;

namespace DeliveryService.Application.Query.PointManager.Models
{
    public class GetRouteByPointOriginAndDestinationQueryRequest
    {
        public string PointOriginCode { get; set; }
        public string PointDestinationCode { get; set; }
    }
}