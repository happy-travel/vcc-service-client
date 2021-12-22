using System;
using System.Collections.Generic;
using HappyTravel.Money.Models;

namespace HappyTravel.VccServiceClient.Models
{
    internal readonly struct VccIssueRequest
    {
        public VccIssueRequest(string referenceCode, MoneyAmount moneyAmount, CreditCardTypes? type,
            DateTime activationDate, DateTime dueDate, Dictionary<string, string> specialValues)
        {
            ReferenceCode = referenceCode;
            MoneyAmount = moneyAmount;
            Type = type;
            ActivationDate = activationDate;
            DueDate = dueDate;
            SpecialValues = specialValues;
        }
        
        
        public string ReferenceCode { get; }
        public MoneyAmount MoneyAmount { get; }
        public CreditCardTypes? Type { get; }
        public DateTime ActivationDate { get; }
        public DateTime DueDate { get; }
        public Dictionary<string, string> SpecialValues { get; }
    }
}