
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Crosscutting.Request.RouteManagement
{
    public class SaveRouteRequest
    {
        [Required]
        public string PointFromCode { get; set; }
        [Required]
        public string PointToCode { get; set; }
        [Required]
        public float Cost { get; set; }
        [Required]
        public float Time { get; set; }
    }
}
