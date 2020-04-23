using SlotsForCourseWork.Models;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using SlotsForCourseWork.DTO;

namespace SlotsForCourseWork.Services.Contracts
{
    public interface ITransactionService
    {
        IQueryable<TransactionDTO> GetAllTransactions();

        IQueryable<TransactionDTO> GetUserTransactionsAsync(string id);

        Task<Transaction> AddTransactionAsync(string userName, int bet, int result);
    }
}