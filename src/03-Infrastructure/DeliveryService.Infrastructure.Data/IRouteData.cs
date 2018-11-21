using DeliveryService.Domain.Model;
using System;
using System.Collections.Generic;

namespace DeliveryService.Infrastructure.Data
{
    public interface IRouteData
    {
        IEnumerable<Route> GetRoutesByPointCodeFrom(string pointFromCode);

        Route GetRoute(string pointCodeFrom, string pointCodeTo);

        void Update(Route route);

        void Save(Route route);

        void Delete(Route route);

        IEnumerable<Route> GetAllRoutes();

        void DeleteRoutesWithPointCode(string code, IEnumerable<string> allPointsCode);
        void DeleteRouteFromCode(string code);
    }
}
