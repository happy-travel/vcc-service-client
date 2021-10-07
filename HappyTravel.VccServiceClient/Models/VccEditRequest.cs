using System;
using HappyTravel.Money.Models;

namespace HappyTravel.VccServiceClient.Models
{
    public struct VccEditRequest
    {
        public DateTime? ActivationDate { get; init; }
        public DateTime? DueDate { get; init; }
        public MoneyAmount? MoneyAmount { get; init; }
    }
}