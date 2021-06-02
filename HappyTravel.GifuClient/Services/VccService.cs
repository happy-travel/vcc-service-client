using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.GifuClient.Models;
using HappyTravel.GifuClient.Options;
using HappyTravel.Money.Models;
using Microsoft.Extensions.Options;

namespace HappyTravel.GifuClient.Services
{
    public class VccService : IVccService
    {
        public VccService(IHttpClientFactory clientFactory, IOptions<GifuHttpClientOptions> options)
        {
            _clientFactory = clientFactory;
            _options = options.Value;
        }
        
        
        public async Task<Result<VirtualCreditCard>> IssueVirtualCreditCard(string referenceCode, MoneyAmount moneyAmount, DateTime dueDate)
        {
            using var client = _clientFactory.CreateClient(HttpClientNames.GifuClient);
            
            var request = new HttpRequestMessage(HttpMethod.Post, _options.Endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(new VccIssueRequest(referenceCode, moneyAmount, dueDate)),
                    Encoding.UTF8, "application/json")
            };
            
            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode
                ? await GetVirtualCreditCard(response.Content)
                : await GetError(response.Content);
        }


        private static async Task<Result<VirtualCreditCard>> GetError(HttpContent content)
        {
            var error = await JsonSerializer.DeserializeAsync<ProblemDetails>(await content.ReadAsStreamAsync());
            return Result.Failure<VirtualCreditCard>(error.Detail);
        }


        private static async Task<Result<VirtualCreditCard>> GetVirtualCreditCard(HttpContent content) 
            => await JsonSerializer.DeserializeAsync<VirtualCreditCard>(await content.ReadAsStreamAsync());


        private readonly IHttpClientFactory _clientFactory;
        private readonly GifuHttpClientOptions _options;
    }
}