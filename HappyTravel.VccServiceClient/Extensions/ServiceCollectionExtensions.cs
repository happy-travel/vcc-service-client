using System;
using System.Net;
using System.Net.Http;
using HappyTravel.VccServiceClient.Options;
using HappyTravel.VccServiceClient.Services;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace HappyTravel.VccServiceClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVccService(this IServiceCollection services, Action<VccClientOptions> options)
        {
            var vccClientOptions = new VccClientOptions();
            options.Invoke(vccClientOptions);

            services.AddHttpClient<IVccService, VccService>(client =>
                {
                    client.BaseAddress = new Uri(GetValueOrThrow(vccClientOptions.VccEndpoint));
                })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                })
                .AddPolicyHandler(vccClientOptions.RetryPolicy ?? GetDefaultRetryPolicy())
                .AddClientAccessTokenHandler(vccClientOptions.IdentityClientName);
            
            return services;
        }
        
        
        private static IAsyncPolicy<HttpResponseMessage> GetDefaultRetryPolicy() 
            => HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(100));


        private static T GetValueOrThrow<T>(T? value) 
            => value is null || value is string str && string.IsNullOrEmpty(str)
                ? throw new ArgumentException($"{nameof(value)} cannot be null") 
                : value;
    }
}