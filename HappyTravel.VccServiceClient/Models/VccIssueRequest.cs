using System;
using HappyTravel.Money.Models;

namespace HappyTravel.VccServiceClient.Models
{
    internal readonly struct VccIssueRequest
    {
        public VccIssueRequest(string referenceCode, MoneyAmount moneyAmount, DateTime activationDate, DateTime dueDate)
        {
            ReferenceCode = referenceCode;
            MoneyAmount = moneyAmount;
            ActivationDate = activationDate;
            DueDate = dueDate;
        }
        
        
        public string ReferenceCode { get; }
        public MoneyAmount MoneyAmount { get; }
        public DateTime ActivationDate { get; }
        public DateTime DueDate { get; }
    }
}