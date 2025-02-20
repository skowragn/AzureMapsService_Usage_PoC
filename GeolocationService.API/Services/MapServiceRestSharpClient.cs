using System.Net;
using GeolocationService.API.Configuration;
using Polly;
using RestSharp;
using GeolocationService.API.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly.CircuitBreaker;
using Polly.Retry;


namespace GeolocationService.API.Services;

public interface IMapServiceRestSharpClient : IMapService;

public class MapServiceRestSharpClient(IOptions<AzureMapsSettings> azureMapSettings) : IMapServiceRestSharpClient
{
    private readonly AzureMapsSettings _azureMapsSettings = azureMapSettings.Value;

    // retry policy
    private static readonly AsyncRetryPolicy<RestResponse> RetryPolicy =
        Policy.HandleResult<RestResponse>(resp =>
                resp.StatusCode == HttpStatusCode.TooManyRequests || (int)resp.StatusCode >= 500)
            .WaitAndRetryAsync(4, retryAttempt =>
            {
                Console.WriteLine($"Attempt {retryAttempt}- Retrying due to server error");
                return TimeSpan.FromSeconds(5 + retryAttempt);
            });

    // circuit breaker policy
    private static readonly AsyncCircuitBreakerPolicy<RestResponse> CircuitBreakerPolicy = Policy
        .HandleResult<RestResponse>(
            resp => (int)resp.StatusCode >= 500)
        .CircuitBreakerAsync(4, TimeSpan.FromSeconds(30));
    

    public async Task<GeolocationResponse?> GetCountryByIp(string ipAddress)
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
        
        var restRequest = new RestRequest($"geolocation/ip/json?subscription-key={azureMapsKey}&api-version=1.0&ip={ipAddress}", Method.Get);

        try
        {
            if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
            {
                throw new Exception("Service not available, thrown intentionally");
            }
            
            var restClient = new RestClient(azureMapsBaseUrl);

            // Normal execution
            //var response = await restClient.ExecuteAsync(restRequest);

            // Retry policy execution
            //var response = await RetryPolicy.ExecuteAsync(async () => await restClient.ExecuteAsync(restRequest));

            // Circuit breaker policy execution
            var response = await CircuitBreakerPolicy.ExecuteAsync(async () => await restClient.ExecuteAsync(restRequest));

            var resultContent = response.Content;

            if (resultContent != null)
            {
                var result = GeolocationResponse(resultContent);
                return result;
            }
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return new GeolocationResponse();
    }

    private static GeolocationResponse? GeolocationResponse(string resultContent)
    {
        var result = JsonConvert.DeserializeObject<GeolocationResponse>(resultContent);
        return result;
    }
}