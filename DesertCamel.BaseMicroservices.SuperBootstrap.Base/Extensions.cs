using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertCamel.BaseMicroservices.SuperBootstrap.Base
{
    public static class Extensions
    {
        public static void AddBootstrapBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.AddScoped<ICorrelationIdUtility, CorrelationIdUtility>();
            services.AddScoped<CorrelationIdMiddleware>();
            services.AddScoped<CorrelationIdLogMiddleware>();
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(policy =>
                {
                    var origins = configuration.GetSection("SuperBootstrap:Cors:AllowedOrigins").Get<string[]>();
                    policy
                        .WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger());
            });
        }

        public static void UseBootstrapBase(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<CorrelationIdLogMiddleware>();
        }
    }
}
