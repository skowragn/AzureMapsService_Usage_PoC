using GeolocationService.API.Services;

namespace GeolocationService.API.Extensions;

public static class AdditionalServices
{
    public static IServiceCollection AddGeolocationServices(this IServiceCollection services)
    {
        services.AddScoped<IMapServiceRestSharpClient, MapServiceRestSharpClient>();
        services.AddScoped<IMapServiceGeolocationClient, MapServiceGeolocationClient>();
        return services;
    }
}