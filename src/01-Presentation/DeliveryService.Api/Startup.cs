using DeliveryService.Crosscutting.Configuration;
using DeliveryService.Infrastructure.Cassandra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.IO;
using DeliveryService.Application.Query;
using DeliveryService.Application.Command;
using DeliveryService.Api.Configs.Swagger;

#if DEBUG
using Microsoft.AspNetCore.Mvc;
#endif

namespace DeliveryService.Api
{
    public class Startup
    {
        private readonly Version _version = Assembly.GetEntryAssembly().GetName().Version;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection sectionCassandra = Configuration.GetSection(AppSettingsConstants.CASSANDRA_CONNECTION);
            services.Configure<CassandraConnectionSetting>(sectionCassandra);

            IConfigurationSection sectionAuth = Configuration.GetSection(AppSettingsConstants.AUTH_CONFIGURATION);
            AuthConfiguration authConfiguration = sectionAuth.Get<AuthConfiguration>();


            services.AddCassandra();
            services.AddApplicationQuery();
            services.AddApplicationCommand();

#if DEBUG
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwagger(_version, authConfiguration);
#else
            services.AddMvcCore().AddJsonFormatters();
#endif

            services.AddAuthentication()
            .AddIdentityServerAuthentication(options =>
                           {
                               options.Authority = authConfiguration.Host;
                               options.RequireHttpsMetadata = false;
                               options.ApiName = authConfiguration.ApiName;
                           });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<SerilogMiddleware>();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseExceptionMiddlewareExtensions();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

#if DEBUG
            app.UseSwaggerConfiguration();
#endif
        }
    }
}
