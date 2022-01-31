using System.Net.Http;
using Polly;

namespace HappyTravel.VccServiceClient.Options
{
    public class VccClientOptions
    {
        public string? VccEndpoint { get; set; }
        public string? IdentityClientName { get; set; }
        public IAsyncPolicy<HttpResponseMessage>? RetryPolicy { get; set; }
    }
}