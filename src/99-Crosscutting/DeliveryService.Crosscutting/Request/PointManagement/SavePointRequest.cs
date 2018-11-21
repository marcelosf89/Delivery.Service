
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Crosscutting.Request.PointManagement
{
    public class SavePointRequest
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
