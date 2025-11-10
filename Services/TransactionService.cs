using Microsoft.EntityFrameworkCore;
using traq_web_project_assessment.Data;
using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public TransactionService(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .ThenInclude(a => a.Person)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .ThenInclude(a => a.Person)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            try
            {
                
                var (isValid, errorMessage) = await ValidateTransactionAsync(transaction); // Validating the transaction
                if (!isValid)
                {
                    return false;
                }

                
                transaction.CaptureDate = DateTime.Now; // Set the date of capture to the current date and time which is now

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                
                await _accountService.UpdateAccountBalanceAsync(transaction.AccountId); // Update the account balance

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            try
            {
                
                var (isValid, errorMessage) = await ValidateTransactionAsync(transaction); // Validating transaction
                if (!isValid)
                {
                    return false;
                }

                 
                transaction.CaptureDate = DateTime.Now; // Updating the date of capture

                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();

                
                await _accountService.UpdateAccountBalanceAsync(transaction.AccountId); // Updating the account balance

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            try
            {
				
                var transaction = await _context.Transactions.FindAsync(id);
                if (transaction == null)
                {
                    return false;
                }

                var accountId = transaction.AccountId;

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();

                
                await _accountService.UpdateAccountBalanceAsync(accountId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(bool isValid, string errorMessage)> ValidateTransactionAsync(Transaction transaction)
        {
            
            if (transaction.TransactionDate > DateTime.Now) // Making sure that the transaction date cannot be set to a time in the future
            {
                return (false, "Transaction date cannot be in the future.");
            }

            // Transaction amount cannot be zero
            if (transaction.DebitAmount == 0 && transaction.CreditAmount == 0)
            {
                return (false, "Transaction amount cannot be zero.");
            }

            // Either debit or credit, not both
            if (transaction.DebitAmount > 0 && transaction.CreditAmount > 0)
            {
                return (false, "Transaction can be either debit or credit, not both.");
            }

            // Check if account is closed
            if (await IsAccountClosedAsync(transaction.AccountId))
            {
                return (false, "Cannot post transactions to closed accounts.");
            }

            return (true, string.Empty);
        }

        public async Task<bool> IsAccountClosedAsync(int accountId)
        {
            var account = await _context.Accounts
                .Include(a => a.Status)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            return account?.Status.StatusName == "Closed";
        }
    }
}