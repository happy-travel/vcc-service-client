using System;
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
            
            services.Configure<HttpClientOptions>(o =>
            {
                o.Endpoint = GetValueOrThrow(vccClientOptions.VccEndpoint);
            });
            
            services.AddAccessTokenManagement(o =>
            {
                o.Client.Clients.Add(HttpClientNames.Identity, new ClientCredentialsTokenRequest
                {
                    Address = GetValueOrThrow(vccClientOptions.IdentityEndpoint),
                    ClientId = GetValueOrThrow(vccClientOptions.IdentityClient),
                    ClientSecret = GetValueOrThrow(vccClientOptions.IdentitySecret)
                });
            });
            
            services.AddHttpClient(HttpClientNames.ApiClient)
                .AddPolicyHandler(vccClientOptions.RetryPolicy ?? GetDefaultRetryPolicy())
                .AddClientAccessTokenHandler(HttpClientNames.Identity);
            
            services.AddTransient<IVccService, VccService>();
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