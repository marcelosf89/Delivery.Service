using System;

namespace DeliveryService.Domain.Model
{
    public class Route
    {
        public string PointFromCode { get; set; }

        public string PointToCode { get; set; }

        public double Cost { get; set; }

        public double Time { get; set; }
    }
}
