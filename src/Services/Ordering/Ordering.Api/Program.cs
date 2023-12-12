
using Common.Logging;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction;
using EvenBus.Messages.IntergrationEvent.Commands.OrderTranSaction.Interfaces;
using Ordering.Api.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Product.Api.Extentions;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName}");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);

    builder.Services.Configure<RouteOptions>(options
     => options.LowercaseUrls = true);
    // Add services to the container.
    builder.Host.AddAppCongigurations();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructure(builder.Configuration);
  


    builder.Services.ConfigureNasstransit();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
   

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    using (var scope = app.Services.CreateScope())
    {
        var orderContextSeed = scope.ServiceProvider.GetRequiredService<OrderContextSeed>();
        await orderContextSeed.InitialiseAsync();
        await orderContextSeed.SeedAsync();
    }

  //  app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapDefaultControllerRoute();

    app.Run();
}
catch(Exception ex)
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


