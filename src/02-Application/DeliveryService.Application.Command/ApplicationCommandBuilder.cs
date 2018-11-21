using DeliveryService.Application.Command.RouteManager;
using DeliveryService.Application.Command.PointManager;
using DeliveryService.Application.Command.PointManager.Models;
using DeliveryService.Crosscutting.Request.RouteManagement;
using DeliveryService.Crosscutting.Request.PointManagement;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryService.Application.Command
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationCommandBuilder
    {
        public static void AddApplicationCommand(this IServiceCollection services)
        {
            services.AddScoped<ICommand<SavePointRequest>, SavePointCommand>();
            services.AddScoped<ICommand<UpdatePointRequest>, UpdatePointCommand>();
            services.AddScoped<ICommand<DeletePointCommnadRequest>, DeletePointCommand>();

            services.AddScoped<ICommand<SaveRouteRequest>, SaveRouteCommand>();
            services.AddScoped<ICommand<UpdateRouteRequest>, UpdateRouteCommand>();
            services.AddScoped<ICommand<DeleteRouteRequest>, DeleteRouteCommand>();
        }
    }
}
