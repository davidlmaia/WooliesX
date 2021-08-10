using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WolliesX.Service
{
    public class WolliesXService : IWolliesXService
    {
        private readonly WolliesXConfiguration wolliesConfiguration;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<WolliesXService> Logger;

        public WolliesXService(IOptions<WolliesXConfiguration> wolliesConfiguration,
            IHttpClientFactory httpClientFactory, 
            ILogger<WolliesXService> Logger)
        {
            this.wolliesConfiguration = wolliesConfiguration.Value;
            this.httpClientFactory = httpClientFactory;
            this.Logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
        }

        public async Task<string> GetResource() 
        {
            var uri = $"{wolliesConfiguration.ServiceEndpoint}/route";
            return "Hello World";
        }
        
        public async Task<string> GetResource(string id)
        {
            var result = string.Empty;

            var uri = $"{wolliesConfiguration.ServiceEndpoint}/route";

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                //Headers = {
                //    { "Accept", "application/json" }
                //},
            };
            
            HttpClient client = httpClientFactory.CreateClient($"WolliesX");

            try
            {
                var response = await client.SendAsync(httpRequestMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to retrieve data from external link:  {uri}";
                Logger.LogError(ex, errorMessage, result);
                throw;
            }


            return result;
        }
    }
}
