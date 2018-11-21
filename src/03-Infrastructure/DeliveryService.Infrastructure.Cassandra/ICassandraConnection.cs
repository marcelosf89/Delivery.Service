using Cassandra;
using System;

namespace DeliveryService.Infrastructure.Cassandra
{
    public interface ICassandraConnection
    {
        ISession GetSession();

        void Rollback(params Func<ISession, bool>[] actions);
    }
}
