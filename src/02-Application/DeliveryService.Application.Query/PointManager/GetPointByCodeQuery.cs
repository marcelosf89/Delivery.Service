using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Crosscutting.Exceptions;
using DeliveryService.Domain.Model;
using DeliveryService.Infrastructure.Data;

namespace DeliveryService.Application.Query.PointManager
{
    public class GetPointByCodeQuery : IQuery<GetPointByCodeQueryRequest, Point>
    {
        private readonly IPointData _pointData;

        public GetPointByCodeQuery(IPointData pointData)
        {
            _pointData = pointData;
        }

        public Point Execute(GetPointByCodeQueryRequest request)
        {
            Point point = _pointData.GetPointByCode(request.Code);

            if (point == null)
            {
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, $"The point does not exist");
            }

            return point;
        }
    }
}
