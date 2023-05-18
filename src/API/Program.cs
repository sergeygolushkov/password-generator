using API.Startup;
using FastEndpoints.Swagger;
using FastEndpoints;
using System.Reflection;
using Serilog;

Environment.CurrentDirectory = AppContext.BaseDirectory;
Environment.SetEnvironmentVariable("BASEDIR", AppContext.BaseDirectory);

// initial bootstrap logger before host is started
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Async(a => a.File("Logs/log.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 10,
        rollOnFileSizeLimit: true,
        //buffered: true,
        //shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(1),
        fileSizeLimitBytes: 10 * 1024 * 1024 // ~10mb
    ), bufferSize: 500)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // the final logger, loads configuration from appsettings
    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
    );

    builder.Services.AddRateLimiter(builder.Configuration);
    builder.Services.AddAppServices();

    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        // change this after tested all configuration to higher value (30 days is default):
        options.MaxAge = TimeSpan.FromHours(1);
        options.ExcludedHosts.Add("example.com");
        options.ExcludedHosts.Add("www.example.com");
    });

    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
    builder.Services.AddFastEndpoints();
    builder.Services.AddSwaggerDoc(maxEndpointVersion: 1,
        settings: settings =>
        {
            settings.Title = "Password Generator API";
            settings.Version = "v1.0";
        },
        addJWTBearerAuth: false,
        excludeNonFastEndpoints: true
    );

    var app = builder.Build();

    app.UseRateLimiter();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    // if exception handler cusomisation would be needed, define middelware:
    // https://github.com/FastEndpoints/FastEndpoints/blob/main/Src/Library/Extensions/ExceptionHandlerExtensions.cs
    app.UseDefaultExceptionHandler();

    app.UseFastEndpoints(c =>
    {
        c.Endpoints.RoutePrefix = "api";
        c.Versioning.Prefix = "v";
        c.Versioning.DefaultVersion = 1;
        c.Versioning.PrependToRoute = true;
    });

    app.UseSwaggerGen();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
