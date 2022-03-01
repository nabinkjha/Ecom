using ECom.Web.Common;
using ECom.Web.Models;
using Microsoft.Extensions.Options;
using Simple.OData.Client;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECom.Web.RESTClients
{
    public abstract class BaseHttpClient
    {
        private readonly HttpClient _httpClient;
        protected IODataClient oDataClient;
        private ApplicationParameters applicationParameters;
        public BaseHttpClient(IOptions<ApplicationParameters> config, HttpClient httpClient)
        {
            applicationParameters = config.Value;
            _httpClient = httpClient;
            var token = GetTokenToAccessOdataAPI().Result;
            var setting = GetSettingWithToken(applicationParameters.ApiUrl, token);
            oDataClient = new ODataClient(setting);
        }
        private async Task<string> GetTokenToAccessOdataAPI()
        {
            _httpClient.DefaultRequestHeaders.Add(applicationParameters.ApiKeyName, applicationParameters.ApiKeyValue);
            var result = await _httpClient.PostAsJsonAsync(applicationParameters.ApiUrl+"Authenticate", new
            {
                UserName = applicationParameters.ApiUserName,
                Password = applicationParameters.ApiPassword
            });
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<ApplicationLoginViewModel>(content);
            return user.token;
        }

        private ODataClientSettings GetSettingWithToken(string url, string accessToken)
        {
            var clientSettings = new ODataClientSettings(new Uri(url));
            clientSettings.BeforeRequest += delegate (HttpRequestMessage message)
            {
                message.Headers.Add("Authorization", "Bearer " + accessToken);
                message.Headers.Add(applicationParameters.ApiKeyName, applicationParameters.ApiKeyValue);
            };
            return clientSettings;
        }
    }
}
