using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Cassandra.Constants;
using Cassandra;

namespace DeliveryService.Infrastructure.Cassandra.Builders
{
    internal static class PointDataBuilderExtension
    {
        internal static  Point BuildPoint(this Row row)
        {
            if (row is null) return null;

            Point point = new Point
            {
                Code = row.GetValue<string>(PointConstant.COLUMNS_CODE),
                Description = row.GetValue<string>(PointConstant.COLUMNS_DESCRIPTION),
            };

            return point;
        }
    }
}
