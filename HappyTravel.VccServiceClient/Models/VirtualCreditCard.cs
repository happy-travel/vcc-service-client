using System;

namespace HappyTravel.VccServiceClient.Models
{
    public readonly struct VirtualCreditCard
    {
        public string Number { get; init; }
        public DateTime Expiry { get; init;}
        public string Holder { get; init;}
        public string Code { get; init;}
        public CreditCardTypes Type { get; init; }
    }
}