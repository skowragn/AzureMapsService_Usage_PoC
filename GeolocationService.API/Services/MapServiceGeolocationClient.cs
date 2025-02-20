using Azure;
using GeolocationService.API.Configuration;
using Microsoft.Extensions.Options;
using Azure.Maps.Geolocation;
using System.Net;
using GeolocationService.API.Model;

namespace GeolocationService.API.Services;

public interface IMapServiceGeolocationClient : IMapService;

public class MapServiceGeolocationClient(IOptions<AzureMapsSettings> azureMapSettings) : IMapServiceGeolocationClient
{
    private readonly AzureMapsSettings _azureMapsSettings = azureMapSettings.Value;
    public async Task<GeolocationResponse?> GetCountryByIp(string ipAddress)
     {
        try
        {
            var azureMapsKey = _azureMapsSettings.AzureMapsSubscriptionKey;
            var azureMapsBaseUrl = _azureMapsSettings.AzureMapsBaseUrl;

            if (string.IsNullOrWhiteSpace(azureMapsKey))
            {
                return new GeolocationResponse();
            }

            if (string.IsNullOrWhiteSpace(azureMapsBaseUrl))
            {
                return new GeolocationResponse();
            }

            var credential = new AzureKeyCredential(azureMapsKey);
            var client = new MapsGeolocationClient(credential);
            var ipAddressIPv6 = IPAddress.Parse(ipAddress);
            
            Response<CountryRegionResult> response = await client.GetCountryCodeAsync(ipAddressIPv6);

            var result = new GeolocationResponse
            {
                CountryRegion = new CountryRegion() { IsoCode = response.Value.IsoCode},
                IpAddress = response.Value.IpAddress.ToString()
            };

            return result;
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
     }
}