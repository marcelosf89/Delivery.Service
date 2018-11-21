
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Crosscutting.Request.PointManagement
{
    public class UpdatePointRequest
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
