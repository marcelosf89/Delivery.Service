using System.Text;

namespace DeliveryService.Infrastructure.Cassandra.Constants
{
    public static class PointConstant
    {
        public static readonly string TABLE_NAME = "points";

        public static string ALL_COLUMNS_EXCEPT_CLAIMS => new StringBuilder().AppendJoin(", ",
                                                            COLUMNS_CODE, COLUMNS_DESCRIPTION).ToString();

        public static readonly string COLUMNS_CODE = "code";
        public static readonly string COLUMNS_DESCRIPTION = "description";
    }
}
