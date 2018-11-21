using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;
using Cassandra;
using System.Linq;
using System.Text;
using DeliveryService.Infrastructure.Cassandra.Constants;
using DeliveryService.Infrastructure.Cassandra.Builders;
using System.Collections.Generic;

namespace DeliveryService.Infrastructure.Cassandra.PointManagement
{
    public class RouteDataCassandra : IRouteData
    {
        private readonly ICassandraConnection _cassandraConnection;

        public RouteDataCassandra(ICassandraConnection cassandraConnection)
        {
            _cassandraConnection = cassandraConnection;
        }

        public void Delete(Route route)
        {
            var statement = new SimpleStatement($"DELETE FROM { RouteConstant.TABLE_NAME } WHERE {RouteConstant.COLUMNS_POINT_FROM_CODE} = :PointFromCode and {RouteConstant.COLUMNS_POINT_TO_CODE} = :PointToCode",
                                                     new
                                                     {
                                                         route.PointFromCode,
                                                         route.PointToCode,
                                                     });

            _cassandraConnection.GetSession().Execute(statement);
        }

        public void DeleteRouteFromCode(string code)
        {
            var session = _cassandraConnection.GetSession();
            var statement = session.Prepare($"DELETE FROM { RouteConstant.TABLE_NAME } WHERE {RouteConstant.COLUMNS_POINT_FROM_CODE} = :PointFromCode")
                .Bind(new
                {
                    PointFromCode = code,
                });


            session.Execute(statement);
        }

        public void DeleteRoutesWithPointCode(string code, IEnumerable<string> allPointsCode)
        {
            var session = _cassandraConnection.GetSession();
            var statement = session.Prepare($"DELETE FROM { RouteConstant.TABLE_NAME } WHERE {RouteConstant.COLUMNS_POINT_FROM_CODE} IN :PointFromCode and {RouteConstant.COLUMNS_POINT_TO_CODE} = :PointToCode")
                .Bind(new
                {
                    PointFromCode = allPointsCode,
                    PointToCode = code,
                });


            session.Execute(statement);
        }

        public IEnumerable<Route> GetAllRoutes()
        {
            var statement = new SimpleStatement($"SELECT {RouteConstant.ALL_COLUMNS_EXCEPT_CLAIMS} FROM { RouteConstant.TABLE_NAME }");

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var rows = rowSet.ToList();

            foreach (var row in rows)
            {
                yield return row.BuildRoute();
            }
        }

        public Route GetRoute(string pointCodeFrom, string pointCodeTo)
        {
            var statement = new SimpleStatement($"SELECT {RouteConstant.ALL_COLUMNS_EXCEPT_CLAIMS} FROM { RouteConstant.TABLE_NAME } WHERE { RouteConstant.COLUMNS_POINT_FROM_CODE } = ? and  { RouteConstant.COLUMNS_POINT_TO_CODE } = ? ", pointCodeFrom, pointCodeTo);

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var row = rowSet.SingleOrDefault();

            return row.BuildRoute();
        }

        public IEnumerable<Route> GetRoutesByPointCodeFrom(string pointCodeFrom)
        {
            var statement = new SimpleStatement($"SELECT {RouteConstant.ALL_COLUMNS_EXCEPT_CLAIMS} FROM { RouteConstant.TABLE_NAME } WHERE { RouteConstant.COLUMNS_POINT_FROM_CODE } = ? ", pointCodeFrom);

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var rows = rowSet.ToList();

            foreach (var row in rows)
            {
                yield return row.BuildRoute();
            }
        }

        public void Save(Route route)
        {
            StringBuilder stringBuilderFields = new StringBuilder();
            stringBuilderFields.Append($"INSERT INTO { RouteConstant.TABLE_NAME } (");
            stringBuilderFields.Append(RouteConstant.ALL_COLUMNS_EXCEPT_CLAIMS);
            stringBuilderFields.Append(") values(:");
            stringBuilderFields.AppendJoin(" ,:",
                                        nameof(route.PointFromCode),
                                        nameof(route.PointToCode),
                                        nameof(route.Cost),
                                        nameof(route.Time));
            stringBuilderFields.Append(")");
            ISession session = _cassandraConnection.GetSession();

            var statement = session.Prepare(stringBuilderFields.ToString());
            session.Execute(statement.Bind(GetDyanmicType(route)));
        }

        public void Update(Route route)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" {0} = :{1},", RouteConstant.COLUMNS_COST, nameof(route.Cost));
            stringBuilder.AppendFormat(" {0} = :{1}", RouteConstant.COLUMNS_TIME, nameof(route.Time));

            ISession session = _cassandraConnection.GetSession();

            var statement = session.Prepare($@"UPDATE {RouteConstant.TABLE_NAME} SET {stringBuilder.ToString()} 
                                               WHERE {RouteConstant.COLUMNS_POINT_FROM_CODE} = :PointFromCode and {RouteConstant.COLUMNS_POINT_TO_CODE} = :PointToCode");

            session.Execute(statement.Bind(GetDyanmicType(route)));
        }

        private object GetDyanmicType(Route route)
        {
            return new
            {
                route.PointFromCode,
                route.PointToCode,
                route.Cost,
                route.Time,
            };

        }
    }
}
