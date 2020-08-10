using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using System.Collections.Generic;

namespace TodoListApp
{   
    public class ForecastService : IForecastService
    {
        private readonly ITokenAcquisition _tokenAcquisition;

        private readonly IConfiguration _configuration;

        private readonly HttpClient _httpClient;

        public ForecastService(
            ITokenAcquisition tokenAcquisition,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Calls the Web API with the required scopes
        /// </summary>
        /// <param name="requireScopes">[Optional] Scopes required to call the Web API. If
        /// not specified, uses scopes from the configuration</param>
        /// <param name="relativeEndpoint">Endpoint relative to the CalledApiUrl configuration</param>
        /// <returns>A JSON string representing the result of calling the Web API</returns>
        public async Task<string> GetForecast(string relativeEndpoint = "", string[] requiredScopes = null)
        {
            string[] scopes = requiredScopes ?? _configuration["WeatherAPI:WeatherScope"]?.Split(' ');
            string apiUrl = (_configuration["WeatherAPI:ApiUrl"] as string)?.TrimEnd('/') + $"/{relativeEndpoint}";

            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            httpRequestMessage.Headers.Add("Authorization", $"bearer {accessToken}");

            string apiResult;
            var response = await _httpClient.SendAsync(httpRequestMessage);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                apiResult = await response.Content.ReadAsStringAsync();
            }
            else
            {
                apiResult = $"Error calling the API '{apiUrl}'";
            }

            return apiResult;
        }
    }
}