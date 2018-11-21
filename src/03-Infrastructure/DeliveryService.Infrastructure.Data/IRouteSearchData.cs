using DeliveryService.Domain.Model;
using System.Collections.Generic;

namespace DeliveryService.Infrastructure.Data
{
    public interface IRouteSearchData
    {
        IEnumerable<Route> GetAllRoutes();

        IEnumerable<Route> GetAllRoutes(string name);

        IEnumerable<Route> GetAllRoutesNextPage(string token);

        void SetRoute(Route route);
    }
}
