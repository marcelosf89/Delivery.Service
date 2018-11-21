using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Cassandra.Constants;
using Cassandra;

namespace DeliveryService.Infrastructure.Cassandra.Builders
{
    internal static class RouteDataBuilderExtension
    {
        internal static  Route BuildRoute(this Row row)
        {
            if (row is null) return null;

            Route route = new Route
            {
                PointFromCode = row.GetValue<string>(RouteConstant.COLUMNS_POINT_FROM_CODE),
                PointToCode = row.GetValue<string>(RouteConstant.COLUMNS_POINT_TO_CODE),
                Cost = row.GetValue<double>(RouteConstant.COLUMNS_COST),
                Time = row.GetValue<double>(RouteConstant.COLUMNS_TIME),
            };

            return route;
        }
    }
}
