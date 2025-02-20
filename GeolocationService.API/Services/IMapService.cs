using GeolocationService.API.Model;

namespace GeolocationService.API.Services;
public interface IMapService
{ 
    Task<GeolocationResponse?> GetCountryByIp(string userIp);
}