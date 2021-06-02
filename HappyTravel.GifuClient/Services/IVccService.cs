using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.GifuClient.Models;
using HappyTravel.Money.Models;

namespace HappyTravel.GifuClient.Services
{
    public interface IVccService
    {
        Task<Result<VirtualCreditCard>> IssueVirtualCreditCard(string referenceCode, MoneyAmount moneyAmount, DateTime dueDate);
    }
}