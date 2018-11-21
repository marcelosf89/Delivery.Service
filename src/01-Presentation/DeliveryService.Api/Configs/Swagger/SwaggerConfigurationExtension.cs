using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Builder;
using DeliveryService.Crosscutting.Configuration;

#if DEBUG
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
#endif

namespace DeliveryService.Api.Configs.Swagger
{
    public static class SwaggerConfigurationExtension
    {
        public static void AddSwagger(this IServiceCollection services, Version version, AuthConfiguration authConfiguration)
        {
#if DEBUG
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Delivery API", Version = $"{version.Major}.{version.Minor}.{version.Build}" });

                c.AddSecurityDefinition("Authorization", new OAuth2Scheme
                {
                    Flow = "implicit",
                    AuthorizationUrl = $"{authConfiguration.Host}/connect/authorize",
                    TokenUrl = $"{authConfiguration.Host}/ connect/token",
                    Type = "oauth2",
                    Scopes = new Dictionary<string, string>()
                    {
                        { "delivery", "Delivery" }
                    }
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } },
                    { "Authorization", new string[] { } },
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
#endif
        }
        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
#if DEBUG
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "{uitemplate}/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery API");
            });

            app.UseReDoc(c =>
            {
                c.RoutePrefix = "api-docs";
                c.SpecUrl = "v1/swagger.json";
            });
#endif
        }

    }
}
