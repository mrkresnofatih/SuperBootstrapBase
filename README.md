# DesertCamel.BaseMicroservices.SuperBootstrap.Base

A Nuget Package for Bootstrapping a .NET Microservice Application with utilities like Logging/Tracing, Cors, Configuration, etc.


## Getting Started

In the `Program.cs` file, write:

```c#
using DesertCamel.BaseMicroservices.SuperIdentity;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();  // important

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, builder.Environment);

app.Run();
```

Your `Startup.cs` file should be implemented as follows:
```c#
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // other services

        services.AddBootstrapBase(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // initial db migration/setup if any

        app.UseRouting();
        app.UseCors();  // remember to always implement this
        app.UseAuthorization();

        app.UseBootstrapBase(); // set this exactly before app.UseEndpoints
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

Add to your `appsettings.json` these in the root level json tree:

```json
{
    "Serilog": {
        "Using": [ "Serilog.Sinks.Console" ],
        "MinimumLevel": {
        "Default": "Information",
        "Override": {
            "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
        }
        },
        "WriteTo": [
            {
                "Name": "Console",
                "Args": {
                "outputTemplate": "{Timestamp:o} [{Level:u4}] [{CorrelationId}] [{SourceContext}] {Message}{NewLine}{Exception}"
                }
            }
        ],
        "Enrich": [ "FromLogContext" ],
        "Destructure": []
  },
  "SuperBootstrap": {
        "Cors": {
            "AllowedOrigins": [
                "http://localhost:5068",
                "http://localhost:3000"
            ]
        }
  }
}
```