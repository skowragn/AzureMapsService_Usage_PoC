using GeolocationService.API.Configuration;

namespace GeolocationService.API.Extensions;

public static class AdditionalConfiguration
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureMapsSettings>(configuration.GetSection(nameof(AzureMapsSettings)));
        services.Configure<DefaultSettings>(configuration.GetSection(nameof(DefaultSettings)));

        return services;
    }
}