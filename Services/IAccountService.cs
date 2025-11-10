using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
       
        Task<Account?> GetAccountByIdAsync(int id);
       
        Task<IEnumerable<Account>> GetAccountsByPersonIdAsync(int personId);
       
        Task<Account?> GetAccountByAccountNumberAsync(string accountNumber);
       
        Task<bool> CreateAccountAsync(Account account);
      
        Task<bool> UpdateAccountAsync(Account account);

        // Closing account if only the balance is 0
        Task<bool> CloseAccountAsync(int id);
       
        Task<bool> CanCloseAccountAsync(int id); // Checking if the account can be closed

        Task<bool> AccountNumberExistsAsync(string accountNumber, int? excludeAccountId = null); // Checking if account number already exists

        Task UpdateAccountBalanceAsync(int accountId); // Update account balance (called when transactions change)
    }
}