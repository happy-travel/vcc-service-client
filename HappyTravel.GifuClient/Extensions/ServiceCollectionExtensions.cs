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
        public static IServiceCollection AddGifuService(this IServiceCollection services, Action<GifuClientOptions> options)
        {
            var gifuClientOptions = new GifuClientOptions();
            options.Invoke(gifuClientOptions);
            
            services.Configure<GifuHttpClientOptions>(o =>
            {
                o.Endpoint = gifuClientOptions.GifuEndpoint;
            });
            
            services.AddAccessTokenManagement(o =>
            {
                o.Client.Clients.Add(HttpClientNames.Identity, new ClientCredentialsTokenRequest
                {
                    Address = gifuClientOptions.IdentityEndpoint,
                    ClientId = gifuClientOptions.IdentityClient,
                    ClientSecret = gifuClientOptions.IdentitySecret
                });
            });
            
            services.AddHttpClient(HttpClientNames.GifuClient)
                .AddPolicyHandler(GetRetryPolicy())
                .AddClientAccessTokenHandler(HttpClientNames.Identity);
            
            services.AddTransient<IGifuService, Services.GifuService>();
            return services;
        }
        
        
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() 
            => HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(100));
    }
}