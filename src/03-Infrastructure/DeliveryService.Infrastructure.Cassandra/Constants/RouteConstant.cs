using System.Text;

namespace DeliveryService.Infrastructure.Cassandra.Constants
{
    public static class RouteConstant
    {
        public static readonly string TABLE_NAME = "routes";

        public static string ALL_COLUMNS_EXCEPT_CLAIMS => new StringBuilder().AppendJoin(", ",
                                                            COLUMNS_POINT_FROM_CODE, COLUMNS_POINT_TO_CODE, COLUMNS_COST, COLUMNS_TIME).ToString();

        public static readonly string COLUMNS_POINT_FROM_CODE = "pointfromcode";
        public static readonly string COLUMNS_POINT_TO_CODE = "pointtocode";
        public static readonly string COLUMNS_COST = "cost";
        public static readonly string COLUMNS_TIME = "time";
    }
}
