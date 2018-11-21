using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Cassandra.Builders;
using DeliveryService.Infrastructure.Cassandra.Constants;
using DeliveryService.Infrastructure.Data;
using Cassandra;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace DeliveryService.Infrastructure.Cassandra.PointManagement
{
    public class PointDataCassandra : IPointData
    {
        private readonly ICassandraConnection _cassandraConnection;
        public PointDataCassandra(ICassandraConnection cassandraConnection)
        {
            _cassandraConnection = cassandraConnection;
        }

        public void Delete(string code)
        {
            var statement = new SimpleStatement($"DELETE FROM { PointConstant.TABLE_NAME } WHERE {PointConstant.COLUMNS_CODE} = ?", code.ToLower());

            _cassandraConnection.GetSession().Execute(statement);
        }

        public IEnumerable<string> GetAllPointCode()
        {
            var statement = new SimpleStatement(
                $"SELECT {PointConstant.COLUMNS_CODE} from { PointConstant.TABLE_NAME }");

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var rows = rowSet.ToList();

            List<string> result = new List<string>();
            foreach (var row in rows)
            {
                result.Add(row.GetValue<string>(PointConstant.COLUMNS_CODE));
            }
            return result;
        }

        public Point GetPointByCode(string code)
        {
            var statement = new SimpleStatement(
                $"SELECT {PointConstant.ALL_COLUMNS_EXCEPT_CLAIMS} from { PointConstant.TABLE_NAME } where {PointConstant.COLUMNS_CODE} = ?", code.ToLower());

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var row = rowSet.SingleOrDefault();

            return row.BuildPoint();
        }

        public void Save(Point point)
        {
            StringBuilder stringBuilderFields = new StringBuilder();
            stringBuilderFields.Append($"INSERT INTO { PointConstant.TABLE_NAME } (");
            stringBuilderFields.Append(PointConstant.ALL_COLUMNS_EXCEPT_CLAIMS);
            stringBuilderFields.Append(") values(:");
            stringBuilderFields.AppendJoin(" ,:", nameof(point.Code), nameof(point.Description));
            stringBuilderFields.Append(")");
            ISession session = _cassandraConnection.GetSession();

            var statement = session.Prepare(stringBuilderFields.ToString());

            session.Execute(statement.Bind(GetDyanmicType(point)));
        }

        public void Update(Point point)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" {0} = :{1}", PointConstant.COLUMNS_DESCRIPTION, nameof(point.Description));

            ISession session = _cassandraConnection.GetSession();

            var statement = session.Prepare($"UPDATE { PointConstant.TABLE_NAME } SET {stringBuilder.ToString()} WHERE {PointConstant.COLUMNS_CODE} = :Code");
            session.Execute(statement.Bind(GetDyanmicType(point)));
        }

        private object GetDyanmicType(Point point)
        {
            return new
            {
                point.Code,
                point.Description
            };
        }

    }
}
