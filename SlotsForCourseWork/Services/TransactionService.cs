using SlotsForCourseWork.Models;
using SlotsForCourseWork.Services.Contracts;
using SlotsForCourseWork.Services.Exceptions;
using SlotsForCourseWork.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationContext context;

        public TransactionService(ApplicationContext context)
        {
            this.context = context ?? throw new ServiceException(nameof(context));
        }

        public IQueryable<TransactionDTO> GetAllTransactions()
        {
            var transactions = this.context.Transactions
                .Select(t => new TransactionDTO
                {
                    Time = t.Time.ToString("MM/dd/yyyy HH:mm"),
                    UserName = t.UserName,
                    Bet = t.Bet,
                    Result= t.Result,
                })
                .AsQueryable();

            return transactions;
        }

        public async Task<Transaction> AddTransactionAsync(string userName, int bet, int result)
        {
            if (userName == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            if (bet < 0)
            {
                throw new ServiceException("Amount must be positive number!");
            }

            var transaction = new Transaction()
            {
                UserName = userName,
                Time = DateTime.Now,
                Bet = bet,
                Result = result
            };

            await this.context.Transactions.AddAsync(transaction);
            await this.context.SaveChangesAsync();

            return transaction;
        }

        public IQueryable<TransactionDTO> GetUserTransactionsAsync(string userName)
        {
            var transactions = this.context.Transactions
               .Where(t => t.UserName == userName)
               .Select(t => new TransactionDTO
               {
                   Time = t.Time.ToString("MM/dd/yyyy HH:mm"),
                   UserName = t.UserName,
                   Bet = t.Bet,
                   Result = t.Result,
               })
               .AsQueryable();

            return transactions;
        }
    }
}
