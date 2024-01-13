using Common.Logging;
using Contract.Common.Interfaces;
using Shared.DTOs.Customer;
using Customer.Api.Persistence;
using Customer.Api.Reoisitories;
using Customer.Api.Reoisitories.Interfaces;
using Customer.Api.Services;
using Customer.Api.Services.Interfaces;
using Infrastructures.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Customer.Api;
using AutoMapper;
using Customer.Api.Entities;
using Customer.Api.Controller;
using Customer.Api.Extensions;
using HealthChecks.UI.Client;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);


Log.Information($"Start {builder.Environment.ApplicationName}");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    // Add services to the container.

    builder.Services.Configure<RouteOptions>(options
        => options.LowercaseUrls = true);

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<CustomerContext>(
        opts => opts.UseNpgsql(connectionString));

    builder.Services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddScoped(typeof(ICustomerRepository), typeof(CustomerRepositotyAsync))
                .AddScoped(typeof(ICustomerService), typeof(CustomerService));
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
    builder.Services.ConfigureHealthChecks();

    var app = builder.Build();

    app.MapCustomerApi();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //  app.UseHttpsRedirection(); // production only
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        endpoints.MapDefaultControllerRoute();
    });

    app.SeedCustomerData();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, $"Unhandlerd exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}







