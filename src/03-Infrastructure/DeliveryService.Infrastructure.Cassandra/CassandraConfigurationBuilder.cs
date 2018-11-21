using DeliveryService.Infrastructure.Cassandra.PointManagement;
using DeliveryService.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryService.Infrastructure.Cassandra
{
    [ExcludeFromCodeCoverage]
   public static class CassandraConfigurationBuilder
    {
        public static void AddCassandra(this IServiceCollection services)
        {
            services.AddSingleton<ICassandraConnection, CassandraConnection>();
            services.AddSingleton<IPointData, PointDataCassandra>();
            services.AddSingleton<IRouteData, RouteDataCassandra>();
        }
    }
}
