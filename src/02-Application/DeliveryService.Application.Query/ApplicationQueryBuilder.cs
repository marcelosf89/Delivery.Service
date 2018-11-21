using DeliveryService.Application.Query.RouteManager;
using DeliveryService.Application.Query.PointManager;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using DeliveryService.Application.Query.Delivery;
using DeliveryService.Domain.Model;
using DeliveryService.Application.Query.PointManager.Models;
using System.Collections.Generic;

namespace DeliveryService.Application.Query
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationQueryBuilder
    {
        public static void AddApplicationQuery(this IServiceCollection services)
        {
            services.AddScoped<IQuery<GetPointByCodeQueryRequest, Point>, GetPointByCodeQuery>();

            services.AddScoped<IQuery<GetRoutesByFromPointCodeQueryRequest, IEnumerable<Route>>, GetRoutesByFromPointCodeQuery>();
            services.AddScoped<IQuery<GetRouteByPointOriginAndDestinationQueryRequest, GetRouteByPointOriginAndDestinationQueryResponse>, GetRouteByPointOriginAndDestinationQuery >();

            services.AddScoped<IQuery<GetBestTimeByPointOriginAndDestinationQueryRequest, IEnumerable<Route>>, GetBestTimeByPointOriginAndDestinationQuery>();
            services.AddScoped<IQuery<GetBestCostByPointOriginAndDestinationQueryRequest, IEnumerable<Route>>, GetBestCostByPointOriginAndDestinationQuery>();
        }
    }
}
