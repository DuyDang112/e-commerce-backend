using Common.Logging;
using Product.Api.Extentions;
using Product.Api.Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName}");
try
{
    builder.Host.UseSerilog(Serilogger.Configure);

    builder.Host.AddAppCongigurations();

    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    app.UseInfrastructure();
    app.MigrateDatabase<ProductContext>((context,_) => // no use IServiceProvider ==> context, _
    {
        ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
    }); // Ihost // auto migrate database when run project

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







