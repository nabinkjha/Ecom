using ECom.Web.Common;
using ECom.Web.Models;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cacheAPIToken ;
        public BaseHttpClient(IOptions<ApplicationParameters> config, HttpClient httpClient, IMemoryCache cacheAPIToken)
        {
            applicationParameters = config.Value;
            _httpClient = httpClient;
            _cacheAPIToken = cacheAPIToken;
            var token = GetTokenToAccessOdataAPI().Result;
            var setting = GetSettingWithToken(applicationParameters.ApiUrl, token);
            oDataClient = new ODataClient(setting);
        }
        private async Task<string> GetTokenToAccessOdataAPI()
        {
            var cacheKey = ("api","token");
            var token = _cacheAPIToken.Get<string>(cacheKey);
            if(token is null)
            {
                _httpClient.DefaultRequestHeaders.Add(applicationParameters.ApiKeyName, applicationParameters.ApiKeyValue);
                var result = await _httpClient.PostAsJsonAsync(applicationParameters.ApiUrl + "Authenticate", new
                {
                    UserName = applicationParameters.ApiUserName,
                    Password = applicationParameters.ApiPassword
                });
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<ApplicationLoginViewModel>(content);
                token= user.token;
                _cacheAPIToken.Set(cacheKey, token, TimeSpan.FromHours(1));
            }
            return token;
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
