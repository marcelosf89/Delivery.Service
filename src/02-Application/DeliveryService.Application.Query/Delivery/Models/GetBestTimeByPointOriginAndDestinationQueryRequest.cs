using System;

namespace DeliveryService.Application.Query.PointManager.Models
{
    public class GetBestTimeByPointOriginAndDestinationQueryRequest
    {
        public string PointOriginCode { get; set; }
        public string PointDestinationCode { get; set; }
    }
}