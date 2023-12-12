using Basket.Api;
using Basket.Api.Extentions;
using Common.Logging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName}");
try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    // configure host
    builder.Host.AddAppCongigurations();
    // configure map appsetting to object
    builder.Services.AddConfigurationSettings(builder.Configuration);

    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
    
    // Add services to the container.
    builder.Services.ConfigureServices();

    builder.Services.Configure<RouteOptions>(options 
        => options.LowercaseUrls = true);

    // configure redis
    builder.Services.ConfigureRedis();
    //configure Grpc
    builder.Services.ConfigureGrpc();
    // configure masstransit
    builder.Services.ConfigureMassTranSit();
    //configure http client
    builder.Services.ConfigureHttpClientService();

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

  //  app.UseHttpsRedirection(); // production only

    app.UseAuthorization();

    app.MapDefaultControllerRoute();

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







