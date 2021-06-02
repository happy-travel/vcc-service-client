using System;
using HappyTravel.Money.Models;

namespace HappyTravel.GifuClient.Models
{
    internal readonly struct VccIssueRequest
    {
        public VccIssueRequest(string referenceCode, MoneyAmount moneyAmount, DateTime dueDate)
        {
            ReferenceCode = referenceCode;
            MoneyAmount = moneyAmount;
            DueDate = dueDate;
        }
        
        
        public string ReferenceCode { get; }
        public MoneyAmount MoneyAmount { get; }
        public DateTime DueDate { get; }
    }
}