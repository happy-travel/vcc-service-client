﻿namespace HappyTravel.VccServiceClient.Options
{
    public class VccClientOptions
    {
        public string VccEndpoint { get; set; } = string.Empty;
        public string IdentityEndpoint { get; set; } = string.Empty;
        public string IdentityClient { get; set; } = string.Empty;
        public string IdentitySecret { get; set; } = string.Empty;
    }
}