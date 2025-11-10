using Microsoft.AspNetCore.Mvc;
using traq_web_project_assessment.Models;
using traq_web_project_assessment.Services;
using traq_web_project_assessment.ViewModels;

namespace traq_web_project_assessment.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;

        public TransactionsController(
            ITransactionService transactionService,
            IAccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }

        // GET: /Transactions/Create?accountId=5
        [HttpGet]
        public async Task<IActionResult> Create(int accountId)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Account not found.";
                return RedirectToAction("Index", "Persons");
            }

            // Check if account is closed
            if (account.Status.StatusName == "Closed")
            {
                TempData["ErrorMessage"] = "Cannot add transactions to a closed account.";
                return RedirectToAction("Details", "Accounts", new { id = accountId });
            }

            ViewBag.AccountNumber = account.AccountNumber;
            ViewBag.AccountName = account.AccountName;
            ViewBag.PersonName = $"{account.Person.FirstName} {account.Person.Surname}";
            ViewBag.CurrentBalance = account.OutstandingBalance;

            var model = new TransactionViewModel
            {
                AccountId = accountId,
                TransactionDate = DateTime.Today,
                AccountNumber = account.AccountNumber,
                PersonName = $"{account.Person.FirstName} {account.Person.Surname}"
            };

            return View(model);
        }

        // POST: /Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel model)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Custom validation: Either debit or credit, not both, not neither
            if (model.DebitAmount == 0 && model.CreditAmount == 0)
            {
                ModelState.AddModelError(string.Empty, "Please enter either a Debit or Credit amount.");
            }

            if (model.DebitAmount > 0 && model.CreditAmount > 0)
            {
                ModelState.AddModelError(string.Empty, "Transaction can be either Debit or Credit, not both.");
            }

            if (!ModelState.IsValid)
            {
                var account = await _accountService.GetAccountByIdAsync(model.AccountId);
                ViewBag.AccountNumber = account.AccountNumber;
                ViewBag.AccountName = account.AccountName;
                ViewBag.PersonName = $"{account.Person.FirstName} {account.Person.Surname}";
                ViewBag.CurrentBalance = account.OutstandingBalance;
                return View(model);
            }

            var transaction = new Transaction
            {
                TransactionDate = model.TransactionDate,
                DebitAmount = model.DebitAmount,
                CreditAmount = model.CreditAmount,
                Description = model.Description,
                AccountId = model.AccountId,
                CaptureDate = DateTime.Now
            };

            // Validate transaction
            var (isValid, errorMessage) = await _transactionService.ValidateTransactionAsync(transaction);
            if (!isValid)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                var account = await _accountService.GetAccountByIdAsync(model.AccountId);
                ViewBag.AccountNumber = account.AccountNumber;
                ViewBag.AccountName = account.AccountName;
                ViewBag.PersonName = $"{account.Person.FirstName} {account.Person.Surname}";
                ViewBag.CurrentBalance = account.OutstandingBalance;
                return View(model);
            }

            var success = await _transactionService.CreateTransactionAsync(transaction);

            if (success)
            {
                TempData["SuccessMessage"] = "Transaction created successfully!";
                return RedirectToAction("Details", "Accounts", new { id = model.AccountId });
            }

            ModelState.AddModelError(string.Empty, "Failed to create transaction. Please try again.");
            var acc = await _accountService.GetAccountByIdAsync(model.AccountId);
            ViewBag.AccountNumber = acc.AccountNumber;
            ViewBag.AccountName = acc.AccountName;
            ViewBag.PersonName = $"{acc.Person.FirstName} {acc.Person.Surname}";
            ViewBag.CurrentBalance = acc.OutstandingBalance;
            return View(model);
        }

        // GET: /Transactions/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                TempData["ErrorMessage"] = "Transaction not found.";
                return RedirectToAction("Index", "Persons");
            }

            var model = new TransactionViewModel
            {
                Id = transaction.Id,
                TransactionDate = transaction.TransactionDate,
                DebitAmount = transaction.DebitAmount,
                CreditAmount = transaction.CreditAmount,
                Description = transaction.Description,
                CaptureDate = transaction.CaptureDate,
                AccountId = transaction.AccountId,
                AccountNumber = transaction.Account.AccountNumber,
                PersonName = $"{transaction.Account.Person.FirstName} {transaction.Account.Person.Surname}"
            };

            ViewBag.AccountNumber = transaction.Account.AccountNumber;
            ViewBag.AccountName = transaction.Account.AccountName;
            ViewBag.PersonName = $"{transaction.Account.Person.FirstName} {transaction.Account.Person.Surname}";

            return View(model);
        }

        // POST: /Transactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransactionViewModel model)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != model.Id)
            {
                return NotFound();
            }

            // Custom validation: Either debit or credit, not both, not neither
            if (model.DebitAmount == 0 && model.CreditAmount == 0)
            {
                ModelState.AddModelError(string.Empty, "Please enter either a Debit or Credit amount.");
            }

            if (model.DebitAmount > 0 && model.CreditAmount > 0)
            {
                ModelState.AddModelError(string.Empty, "Transaction can be either Debit or Credit, not both.");
            }

            if (!ModelState.IsValid)
            {
                var account = await _accountService.GetAccountByIdAsync(model.AccountId);
                ViewBag.AccountNumber = account.AccountNumber;
                ViewBag.AccountName = account.AccountName;
                ViewBag.PersonName = $"{account.Person.FirstName} {account.Person.Surname}";
                return View(model);
            }

            var transaction = new Transaction
            {
                Id = model.Id,
                TransactionDate = model.TransactionDate,
                DebitAmount = model.DebitAmount,
                CreditAmount = model.CreditAmount,
                Description = model.Description,
                AccountId = model.AccountId,
                CaptureDate = DateTime.Now // Update capture date
            };

            // Validate transaction
            var (isValid, errorMessage) = await _transactionService.ValidateTransactionAsync(transaction);
            if (!isValid)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                var account = await _accountService.GetAccountByIdAsync(model.AccountId);
                ViewBag.AccountNumber = account.AccountNumber;
                ViewBag.AccountName = account.AccountName;
                ViewBag.PersonName = $"{account.Person.FirstName} {account.Person.Surname}";
                return View(model);
            }

            var success = await _transactionService.UpdateTransactionAsync(transaction);

            if (success)
            {
                TempData["SuccessMessage"] = "Transaction updated successfully!";
                return RedirectToAction("Details", "Accounts", new { id = model.AccountId });
            }

            ModelState.AddModelError(string.Empty, "Failed to update transaction. Please try again.");
            var acc = await _accountService.GetAccountByIdAsync(model.AccountId);
            ViewBag.AccountNumber = acc.AccountNumber;
            ViewBag.AccountName = acc.AccountName;
            ViewBag.PersonName = $"{acc.Person.FirstName} {acc.Person.Surname}";
            return View(model);
        }

        // GET: /Transactions/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                TempData["ErrorMessage"] = "Transaction not found.";
                return RedirectToAction("Index", "Persons");
            }

            var model = new TransactionViewModel
            {
                Id = transaction.Id,
                TransactionDate = transaction.TransactionDate,
                DebitAmount = transaction.DebitAmount,
                CreditAmount = transaction.CreditAmount,
                Description = transaction.Description,
                CaptureDate = transaction.CaptureDate,
                AccountId = transaction.AccountId,
                AccountNumber = transaction.Account.AccountNumber,
                PersonName = $"{transaction.Account.Person.FirstName} {transaction.Account.Person.Surname}"
            };

            return View(model);
        }

        // POST: /Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                TempData["ErrorMessage"] = "Transaction not found.";
                return RedirectToAction("Index", "Persons");
            }

            var accountId = transaction.AccountId;
            var success = await _transactionService.DeleteTransactionAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Transaction deleted successfully!";
                return RedirectToAction("Details", "Accounts", new { id = accountId });
            }

            TempData["ErrorMessage"] = "Failed to delete transaction.";
            return RedirectToAction("Delete", new { id });
        }
    }
}