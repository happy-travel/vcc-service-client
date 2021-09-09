using System;
using System.Collections.Generic;
using HappyTravel.Money.Models;

namespace HappyTravel.VccServiceClient.Models
{
    internal readonly struct VccIssueRequest
    {
        public VccIssueRequest(string referenceCode, MoneyAmount moneyAmount, DateTime activationDate, DateTime dueDate,
            Dictionary<string, string> specialValues)
        {
            ReferenceCode = referenceCode;
            MoneyAmount = moneyAmount;
            ActivationDate = activationDate;
            DueDate = dueDate;
            SpecialValues = specialValues;
        }
        
        
        public string ReferenceCode { get; }
        public MoneyAmount MoneyAmount { get; }
        public DateTime ActivationDate { get; }
        public DateTime DueDate { get; }
        public Dictionary<string, string> SpecialValues { get; }
    }
}