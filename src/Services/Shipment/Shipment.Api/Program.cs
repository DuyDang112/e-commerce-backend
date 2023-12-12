using Common.Logging;
using Serilog;
using Shipment.Api.Extentions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName}");

try
{
    builder.Host.AddAppCongigurations();
    builder.Host.UseSerilog(Serilogger.Configure);

    builder.Services.Configure<RouteOptions>(options
     => options.LowercaseUrls = true);

    // Add services to the container.

    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddInfrastructureServices();
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

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

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