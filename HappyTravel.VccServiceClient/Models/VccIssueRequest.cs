using System;
using System.Collections.Generic;
using HappyTravel.Money.Models;

namespace HappyTravel.VccServiceClient.Models
{
    internal readonly struct VccIssueRequest
    {
        public VccIssueRequest(string referenceCode, MoneyAmount moneyAmount, List<CreditCardTypes>? types,
            DateTimeOffset activationDate, DateTimeOffset dueDate, Dictionary<string, string> specialValues)
        {
            ReferenceCode = referenceCode;
            MoneyAmount = moneyAmount;
            Types = types;
            ActivationDate = activationDate;
            DueDate = dueDate;
            SpecialValues = specialValues;
        }
        
        
        public string ReferenceCode { get; }
        public MoneyAmount MoneyAmount { get; }
        public List<CreditCardTypes>? Types { get; }
        public DateTimeOffset ActivationDate { get; }
        public DateTimeOffset DueDate { get; }
        public Dictionary<string, string> SpecialValues { get; }
    }
}