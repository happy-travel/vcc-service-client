using System.Net.Http;
using Polly;

namespace HappyTravel.VccServiceClient.Options
{
    public class VccClientOptions
    {
        public string? VccEndpoint { get; set; }
        public string? IdentityEndpoint { get; set; }
        public string? IdentityClient { get; set; }
        public string? IdentitySecret { get; set; }
        public IAsyncPolicy<HttpResponseMessage>? RetryPolicy { get; set; }
    }
}