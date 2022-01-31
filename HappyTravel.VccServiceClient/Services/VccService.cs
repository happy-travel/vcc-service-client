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
        public VccService(HttpClient client)
        {
            _client = client;
        }
        
        
        public async Task<Result<VirtualCreditCard>> IssueVirtualCreditCard(string referenceCode, MoneyAmount moneyAmount, List<CreditCardTypes>? types,
            DateTimeOffset activationDate, DateTimeOffset dueDate, Dictionary<string, string> specialValues)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, string.Empty)
            {
                Content = new StringContent(JsonSerializer.Serialize(new VccIssueRequest(referenceCode: referenceCode, 
                        moneyAmount: moneyAmount, 
                        types: types,
                        activationDate: activationDate, 
                        dueDate: dueDate, 
                        specialValues: specialValues)),
                    Encoding.UTF8, "application/json")
            };
            
            var response = await _client.SendAsync(request);

            return response.IsSuccessStatusCode
                ? await GetVirtualCreditCard(response.Content)
                : await GetError(response.Content);
        }

        
        public async Task<Result> ModifyAmount(string referenceCode, MoneyAmount amount)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{referenceCode}")
            {
                Content = new StringContent(JsonSerializer.Serialize(amount), Encoding.UTF8, "application/json")
            };
            
            using var response = await _client.SendAsync(request);

            return response.IsSuccessStatusCode
                ? Result.Success()
                : await GetError(response.Content);
        }

        
        public async Task<Result> Delete(string referenceCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{referenceCode}");
            using var response = await _client.SendAsync(request);

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

        private readonly HttpClient _client;
    }
}