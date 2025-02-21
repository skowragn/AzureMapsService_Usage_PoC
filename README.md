# AzureMapsService_Usage_PoC
The Azure Maps Service API usage to find user country by IP.

# 1. Overview
The Azure Maps is a collection of geospatial services and SDKs that use fresh mapping data to provide geographic context to web and mobile applications. 
There are the following features:
 - REST APIs to render vector and raster maps in multiple styles and satellite imagery
 - Search services to locate addresses, places, and points of interest around the world
 - Various routing options; such as point-to-point, multipoint, multipoint optimization, isochrone, electric vehicle, commercial vehicle, traffic influenced, and matrix routing
 - Traffic flow view and incidents view, for applications that require real-time traffic information
 - Time zone and Geolocation services
 - Geofencing service with location information hosted in Azure
 - Location intelligence through geospatial analytics

from: https://learn.microsoft.com/en-us/azure/azure-maps/about-azure-maps

# 2. Constrains

 1. Azure Maps Batches Search Address API:  the API limits the batch size to 10000 queries per request.

 2. The Azure Portal free account allows up to 250,000 transactions per month.

# 3. Solution overview
In the source code example -> the Geolocation service is using with the following API:
```
https://atlas.microsoft.com/geolocation/ip/json?subscription-key={azureMapsKey}&api-version=1.0&ip={ipAddress}
```
to find user country by IP.

As the default USA IPv6 address is using: 2001:4898:80e8:b::189 (from Microsoft example).    

## 3.1. How to start
1. Use/Create an account in  Azure Portal (https://portal.azure.com)

![image](https://github.com/user-attachments/assets/a20521c3-39e5-469a-8cb6-48b22411286d)
   
2.Create Azure Maps service

![image](https://github.com/user-attachments/assets/6f7df8d1-23e0-45f0-acb1-4b7c272319fd)


3. Authentication: Recommendation: Entra ID, but in the example: API Key is using (Shared Key Authentication) - Primary Key or Secondary Key

 ![image](https://github.com/user-attachments/assets/b2203110-0196-4cb5-9e5f-82b2b5111119)

4. Copy Primary Key or Secondary Key into appsettings.json:

```
"AzureMapsSettings": {
    "AzureMapsSubscriptionKey": "{Primary Key}",
    "AzureMapsBaseUrl": "https://atlas.microsoft.com/"
}
```
5. There are two endpoints:

```
https://localhost:7218/api/AzureMaps/geolocation/{ipAddress?}
```

```
https://localhost:7218/api/AzureMaps/restsharp/{ipAddress?}
```

https://learn.microsoft.com/en-us/rest/api/maps/geolocation/get-ip-to-location?view=rest-maps-2024-04-01&tabs=HTTP

![image](https://github.com/user-attachments/assets/8028e10c-00b9-4a9c-b0be-3514535179c7)

The first one is using MapsGeolocationClient client from Azure.Maps.Geolocation NuGet.

The second one is using RestSharp client to call directly the following API:

```
https://atlas.microsoft.com/geolocation/ip/json?subscription-key={azureMapsKey}&api-version=1.0&ip={ipAddress}
```

6. Result and API response model:

```
https://localhost:7218/swagger
```

![image](https://github.com/user-attachments/assets/8028e10c-00b9-4a9c-b0be-3514535179c7)

![image](https://github.com/user-attachments/assets/057c357e-037f-40ac-86c9-c655c1b1f3dd)

![image](https://github.com/user-attachments/assets/caf3eed0-b21d-49f7-80ec-a0b1af72cd2a)



```
https://localhost:7218/scalar
```

![image](https://github.com/user-attachments/assets/147d80c3-e2cd-4fcd-9dd5-2dca4f52fb2c)

![image](https://github.com/user-attachments/assets/e398d1be-3a1f-4faf-8102-3b44a2e02e03)

![image](https://github.com/user-attachments/assets/7d018e84-932a-4ea7-b0ed-9034130190f6)


# 4. References

https://learn.microsoft.com/en-us/azure/azure-maps/how-to-dev-guide-csharp-sdk 

https://learn.microsoft.com/en-us/rest/api/maps/geolocation/get-ip-to-location?view=rest-maps-2024-04-01&tabs=HTTP 

https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/maps/Azure.Maps.Geolocation#getting-started 

https://learn.microsoft.com/en-us/dotnet/api/overview/azure/maps.geolocation-readme?view=azure-dotnet-preview 
  
