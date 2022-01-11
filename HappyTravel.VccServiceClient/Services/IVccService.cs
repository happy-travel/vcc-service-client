using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Money.Models;
using HappyTravel.VccServiceClient.Models;

namespace HappyTravel.VccServiceClient.Services
{
    public interface IVccService
    {
        Task<Result<VirtualCreditCard>> IssueVirtualCreditCard(string referenceCode, MoneyAmount moneyAmount, List<CreditCardTypes>? types,
            DateTime activationDate, DateTime dueDate, Dictionary<string, string> specialValues);

        Task<Result> ModifyAmount(string referenceCode, MoneyAmount amount);
        
        Task<Result> Delete(string referenceCode);
    }
}