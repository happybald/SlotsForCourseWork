using SlotsForCourseWork.Models;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using SlotsForCourseWork.DTO;

namespace SlotsForCourseWork.Services.Contracts
{
    public interface ITransactionService
    {
        IQueryable<TransactionDto> GetAllTransactions();

        IQueryable<TransactionDto> GetUserTransactionsAsync(string id);

        Transaction AddTransaction(string userName, int bet, int result);
    }
}