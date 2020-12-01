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
        private readonly ApplicationContext _context;

        public TransactionService(ApplicationContext context)
        {
            this._context = context ?? throw new ServiceException(nameof(context));
        }

        public IQueryable<TransactionDto> GetAllTransactions()
        {
            return _context.Transactions
                .OrderByDescending(t => t.Time)
                .Take(20)
                .Select(t => new TransactionDto(t.UserName, t.Time.ToString("MM/dd/yyyy HH:mm"), t.Bet, t.Result));
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

            _context.Transactions.AddAsync(transaction);
            _context.SaveChangesAsync();
            return transaction;
        }

        public IQueryable<TransactionDto> GetUserTransactionsAsync(string userName)
        {
            return _context.Transactions
               .Where(t => t.UserName == userName)
               .Select(t => new TransactionDto(t.UserName, t.Time.ToString("MM/dd/yyyy HH:mm"), t.Bet, t.Result));
        }
    }
}
