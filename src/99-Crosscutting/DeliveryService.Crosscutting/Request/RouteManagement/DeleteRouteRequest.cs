using System;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Crosscutting.Request.RouteManagement
{
    public class DeleteRouteRequest
    {
        [Required]
        public string PointFromCode { get; set; }
        [Required]
        public string PointToCode { get; set; }
    }
}
