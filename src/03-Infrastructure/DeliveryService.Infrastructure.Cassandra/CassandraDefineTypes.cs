using DeliveryService.Domain.Model;
using Cassandra;
using System.Diagnostics.CodeAnalysis;
using DeliveryService.Infrastructure.Cassandra.Constants;

namespace DeliveryService.Infrastructure.Cassandra
{
    [ExcludeFromCodeCoverage]
    public static class CassandraDefineTypes
    {
        public static void AddUDTType(this ISession session)
        {
        }
    }
}
