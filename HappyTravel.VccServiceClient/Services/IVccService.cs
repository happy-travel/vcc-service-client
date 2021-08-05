using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Money.Models;
using HappyTravel.VccServiceClient.Models;

namespace HappyTravel.VccServiceClient.Services
{
    public interface IVccService
    {
        Task<Result<VirtualCreditCard>> IssueVirtualCreditCard(string referenceCode, MoneyAmount moneyAmount, DateTime startDate, DateTime dueDate);
    }
}