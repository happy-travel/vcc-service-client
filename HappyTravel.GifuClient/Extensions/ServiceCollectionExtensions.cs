using System;
using System.Net.Http;
using HappyTravel.GifuClient.Options;
using HappyTravel.GifuClient.Services;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace HappyTravel.GifuClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVccService(this IServiceCollection services, Action<VccClientOptions> options)
        {
            var vccClientOptions = new VccClientOptions();
            options.Invoke(vccClientOptions);
            
            services.Configure<HttpClientOptions>(o =>
            {
                o.Endpoint = vccClientOptions.VccEndpoint;
            });
            
            services.AddAccessTokenManagement(o =>
            {
                o.Client.Clients.Add(HttpClientNames.Identity, new ClientCredentialsTokenRequest
                {
                    Address = vccClientOptions.IdentityEndpoint,
                    ClientId = vccClientOptions.IdentityClient,
                    ClientSecret = vccClientOptions.IdentitySecret
                });
            });
            
            services.AddHttpClient(HttpClientNames.ApiClient)
                .AddPolicyHandler(GetRetryPolicy())
                .AddClientAccessTokenHandler(HttpClientNames.Identity);
            
            services.AddTransient<IVccService, VccService>();
            return services;
        }
        
        
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() 
            => HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(100));
    }
}