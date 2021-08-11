using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WolliesX.Service.Common;
using WolliesX.Service.Models;
using WolliesX.Service.Models.v1;
using WolliesX.Service.Models.v1.Trolley;

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

        public async Task<Result<string>> GetResource() 
        {
            return new Result<string>("Hello Worldddddd");
        }
        
        public async Task<Result<IEnumerable<Product>>> GetSortedProducts(string sortOption)
        {
            var uri = $"{wolliesConfiguration.ServiceEndpoint}/products?token={wolliesConfiguration.Token}";

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers = {
                    { "Accept", "application/json" }
                },
            };
            
            HttpClient client = httpClientFactory.CreateClient($"WolliesX");

            try
            {
                var response = await client.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();
                var shoppingList = JsonConvert.DeserializeObject<List<Product>>(responseContent);
                
                var result = await SortProductsByOption(sortOption, shoppingList);
                return result;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to retrieve data from external link:  {uri}";
                Logger.LogError(ex, errorMessage, null);
                return null;
            }

        }

        private async Task<Result<IEnumerable<Product>>> SortProductsByOption(string sortOption, IEnumerable<Product> products)
        {
            if (string.IsNullOrEmpty(sortOption))
            {
                return new Result<IEnumerable<Product>>(new ApiError("Sort Option Required", "No sorting option was provided"));
            }

            IEnumerable<Product> sortedProducts = Enumerable.Empty<Product>();

            switch (sortOption)
            {
                case "low":
                    sortedProducts = products.OrderBy(p => p.Price).ToList();
                    break;
                case "high":
                    sortedProducts = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "ascending":
                    sortedProducts = products.OrderBy(p => p.Name).ToList();
                    break;
                case "descending":
                    sortedProducts = products.OrderByDescending(p => p.Name).ToList();
                    break;
                case "recommended":
                    sortedProducts = await GetShopperHistory();
                    break;
                default:
                    return new Result<IEnumerable<Product>>(new ApiError("Invalid Sort Option", "Sort option does not match available types (high, low, ascending, descending, recommended)"));
            }

            return new Result<IEnumerable<Product>>(sortedProducts);
        }

        private async Task<IEnumerable<Product>> GetShopperHistory() 
        {
            var result = string.Empty;

            var uri = $"{wolliesConfiguration.ServiceEndpoint}/shopperHistory?token={wolliesConfiguration.Token}";

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers = {
                    { "Accept", "application/json" }
                },
            };

            HttpClient client = httpClientFactory.CreateClient($"WolliesX");

            try
            {
                Logger.LogInformation($"Attempting to retrieve data from external api {uri}");

                var response = await client.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();
                var shoppingList = JsonConvert.DeserializeObject<List<Order>>(responseContent);

                var products = shoppingList.SelectMany(x => x.Products)
                    .GroupBy(p=>p.Name)
                    .OrderByDescending(p=>p.Count())
                    .Select(x => new Product { Name = x.Key, Quantity = 0, Price = x.FirstOrDefault(y=>y.Name == x.Key).Price }).Distinct();
                
                return products;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to retrieve data from external api:  {uri}";
                Logger.LogError(ex, errorMessage, result);
                //Ideally would be looging into appInsights 
                return null;
            }
        }

        public async Task<Result<double>> GetTrolleyTotal(Trolley trolley)
        {
            var total = TrolleyCalculator.CalculateTrolley(trolley);

            return new Result<double>(total);
        }

    }
}
