using Microsoft.EntityFrameworkCore;
using traq_web_project_assessment.Data;
using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .Include(a => a.Person)
                .Include(a => a.Status)
                .Include(a => a.Transactions)
                .OrderBy(a => a.AccountNumber)
                .ToListAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .Include(a => a.Person)
                .Include(a => a.Status)
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Account>> GetAccountsByPersonIdAsync(int personId)
        {
            return await _context.Accounts
                .Include(a => a.Status)
                .Include(a => a.Transactions)
                .Where(a => a.PersonId == personId)
                .OrderBy(a => a.AccountNumber)
                .ToListAsync();
        }

        public async Task<Account?> GetAccountByAccountNumberAsync(string accountNumber)
        {
            return await _context.Accounts
                .Include(a => a.Person)
                .Include(a => a.Status)
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task<bool> CreateAccountAsync(Account account)
        {
            try
            {
				
                
                if (await AccountNumberExistsAsync(account.AccountNumber)) // Check to see if the account number is already existing
                {
                    return false;
                }

                
                account.OutstandingBalance = 0; //Inital balance equals 0

                // Set default status to Open (Id = 1)
                account.StatusId = 1;

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            try
            {
                // Check if account number already exists for another account
                if (await AccountNumberExistsAsync(account.AccountNumber, account.Id))
                {
                    return false;
                }

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CloseAccountAsync(int id)
        {
            try
            {
                if (!await CanCloseAccountAsync(id))
                {
                    return false;
                }

                var account = await _context.Accounts.FindAsync(id);
                if (account == null)
                {
                    return false;
                }

                
                account.StatusId = 2; // Setting the status to Closed meaning Id = 2

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CanCloseAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return false;
            }

            
            return account.OutstandingBalance == 0; // It can only close if and when the balance is equal to zero
        }

        public async Task<bool> AccountNumberExistsAsync(string accountNumber, int? excludeAccountId = null)
        {
            if (excludeAccountId.HasValue)
            {
                return await _context.Accounts
                    .AnyAsync(a => a.AccountNumber == accountNumber && a.Id != excludeAccountId.Value);
            }

            return await _context.Accounts
                .AnyAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task UpdateAccountBalanceAsync(int accountId)
        {
            var account = await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null)
            {
                return;
            }

            // Calculate balance: sum credits - sum debits
            var totalCredits = account.Transactions.Sum(t => t.CreditAmount);
            var totalDebits = account.Transactions.Sum(t => t.DebitAmount);

            account.OutstandingBalance = totalCredits - totalDebits;

            await _context.SaveChangesAsync();
        }
    }
}