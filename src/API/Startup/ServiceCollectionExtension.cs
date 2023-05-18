using System.Threading.RateLimiting;
using API.ConfigurationEntities;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Startup;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAppServices(
        this IServiceCollection services)
    {
        services.AddScoped<IPasswordCreatorService, PasswordCreatorService>();

        return services;
    }

    public static IServiceCollection AddRateLimiter(
        this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitOptions = new RateLimitOptions();
        configuration.GetSection(RateLimitOptions.ConfigurationSectionName).Bind(rateLimitOptions);
        var fixedPolicy = "fixed";
        services.AddRateLimiter(limiterOptions =>
        {
            limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            limiterOptions.AddFixedWindowLimiter(policyName: fixedPolicy, options =>
            {
                options.PermitLimit = rateLimitOptions.PermitLimit;
                options.Window = TimeSpan.FromSeconds(rateLimitOptions.WindowSeconds);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = rateLimitOptions.QueueLimit;
            });
        });
        return services;
    }
}
