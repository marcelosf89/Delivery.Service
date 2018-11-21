using System;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Crosscutting.Request.RouteManagement
{
    public class UpdateRouteRequest
    {
        [Required]
        public string PointFromCode { get; set; }
        [Required]
        public string PointToCode { get; set; }
        public Nullable<float> Cost { get; set; }
        public Nullable<float> Time { get; set; }
    }
}
