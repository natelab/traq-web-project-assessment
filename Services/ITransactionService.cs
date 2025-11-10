using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();

        Task<Transaction?> GetTransactionByIdAsync(int id);

        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);

        Task<bool> CreateTransactionAsync(Transaction transaction); // Create a new transaction

        Task<bool> UpdateTransactionAsync(Transaction transaction);

        Task<bool> DeleteTransactionAsync(int id);
  
        Task<(bool isValid, string errorMessage)> ValidateTransactionAsync(Transaction transaction); // Validating a transaction by making use of the business rules

        Task<bool> IsAccountClosedAsync(int accountId); // Checking if account has been closed
    }
}