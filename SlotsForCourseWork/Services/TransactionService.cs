using SlotsForCourseWork.Models;
using SlotsForCourseWork.Services.Contracts;
using SlotsForCourseWork.Services.Exceptions;
using SlotsForCourseWork.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace SlotsForCourseWork.Services
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
            return context.Transactions
                .OrderByDescending(t => t.Time)
                .Take(20)
                .Select(t => new TransactionDTO(t.UserName, t.Time.ToString("MM/dd/yyyy HH:mm"), t.Bet, t.Result));
        }

        public Transaction AddTransaction(string userName, int bet, int result)
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

            context.Transactions.AddAsync(transaction);
            context.SaveChangesAsync();
            return transaction;
        }

        public IQueryable<TransactionDTO> GetUserTransactionsAsync(string userName)
        {
            return context.Transactions
               .Where(t => t.UserName == userName)
               .Select(t => new TransactionDTO(t.UserName, t.Time.ToString("MM/dd/yyyy HH:mm"), t.Bet, t.Result));
        }
    }
}
