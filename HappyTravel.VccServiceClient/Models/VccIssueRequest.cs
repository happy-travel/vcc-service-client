using System;
using HappyTravel.Money.Models;

namespace HappyTravel.VccServiceClient.Models
{
    internal readonly struct VccIssueRequest
    {
        public VccIssueRequest(string referenceCode, MoneyAmount moneyAmount, DateTime startDate, DateTime dueDate)
        {
            ReferenceCode = referenceCode;
            MoneyAmount = moneyAmount;
            StartDate = startDate;
            DueDate = dueDate;
        }
        
        
        public string ReferenceCode { get; }
        public MoneyAmount MoneyAmount { get; }
        public DateTime StartDate { get; }
        public DateTime DueDate { get; }
    }
}