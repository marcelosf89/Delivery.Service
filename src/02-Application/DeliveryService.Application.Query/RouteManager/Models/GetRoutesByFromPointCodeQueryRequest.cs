using System;

namespace DeliveryService.Application.Query.PointManager.Models
{
    public class GetRoutesByFromPointCodeQueryRequest
    {
        public string PointFromCode { get; set; }
    }
}