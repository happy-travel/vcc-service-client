using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Money.Models;
using HappyTravel.VccServiceClient.Models;
using HappyTravel.VccServiceClient.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HappyTravel.VccServiceClient.Services
{
    public class VccService : IVccService
    {
        public VccService(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options)
        {
            _clientFactory = clientFactory;
            _options = options.Value;
        }
        
        
        public async Task<Result<VirtualCreditCard>> IssueVirtualCreditCard(string referenceCode, MoneyAmount moneyAmount, List<CreditCardTypes>? types,
            DateTime activationDate, DateTime dueDate, Dictionary<string, string> specialValues)
        {
            using var client = _clientFactory.CreateClient(HttpClientNames.ApiClient);
            
            var request = new HttpRequestMessage(HttpMethod.Post, _options.Endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(new VccIssueRequest(referenceCode: referenceCode, 
                        moneyAmount: moneyAmount, 
                        types: types,
                        activationDate: activationDate, 
                        dueDate: dueDate, 
                        specialValues: specialValues)),
                    Encoding.UTF8, "application/json")
            };
            
            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode
                ? await GetVirtualCreditCard(response.Content)
                : await GetError(response.Content);
        }

        
        public async Task<Result> ModifyAmount(string referenceCode, MoneyAmount amount)
        {
            using var client = _clientFactory.CreateClient(HttpClientNames.ApiClient);
            
            var request = new HttpRequestMessage(HttpMethod.Put, $"{_options.Endpoint}/{referenceCode}")
            {
                Content = new StringContent(JsonSerializer.Serialize(amount), Encoding.UTF8, "application/json")
            };
            
            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode
                ? Result.Success()
                : await GetError(response.Content);
        }

        
        public async Task<Result> Delete(string referenceCode)
        {
            using var client = _clientFactory.CreateClient(HttpClientNames.ApiClient);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_options.Endpoint}/{referenceCode}");
            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode
                ? Result.Success()
                : await GetError(response.Content);
        }


        private static async Task<Result<VirtualCreditCard>> GetError(HttpContent content)
        {
            string error;

            try
            {
                var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(await content.ReadAsStreamAsync(), SerializerOptions);
                error = problemDetails?.Detail ?? string.Empty;
            }
            catch (JsonException)
            {
                error = await content.ReadAsStringAsync();
            }
            
            return Result.Failure<VirtualCreditCard>(error);
        }


        private static async Task<Result<VirtualCreditCard>> GetVirtualCreditCard(HttpContent content) 
            => await JsonSerializer.DeserializeAsync<VirtualCreditCard>(await content.ReadAsStreamAsync(), SerializerOptions);


        private static readonly JsonSerializerOptions SerializerOptions = new () { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClientOptions _options;
    }
}