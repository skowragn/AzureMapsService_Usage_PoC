using GeolocationService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeolocationService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AzureMapsController(IMapServiceGeolocationClient mapServiceGeolocationClient, IMapServiceRestSharpClient mapServiceRestSharpClient) : ControllerBase
{
    [HttpGet("/geolocation/{ipAddress?}")]
    public async Task<IActionResult> GetByAzureGeolocation(string ipAddress = "2001:4898:80e8:b::189")
    {
        var country = await mapServiceGeolocationClient.GetCountryByIp(ipAddress);
        return Ok(country);
    }


    [HttpGet("/restsharp/{ipAddress?}")]
    public async Task<IActionResult> GetByRestSharp(string ipAddress = "2001:4898:80e8:b::189")
    {
        var country = await mapServiceRestSharpClient.GetCountryByIp(ipAddress);
        return Ok(country);
    }
}