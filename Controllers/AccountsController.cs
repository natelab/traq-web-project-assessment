using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using traq_web_project_assessment.Models;
using traq_web_project_assessment.Services;
using traq_web_project_assessment.ViewModels;

//This file manages all the bank account operations
namespace traq_web_project_assessment.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IPersonService _personService;
        private readonly ITransactionService _transactionService;

        public AccountsController(
            IAccountService accountService,
            IPersonService personService,
            ITransactionService transactionService)
        {
            _accountService = accountService;
            _personService = personService;
            _transactionService = transactionService;
        }

        // GET: /Accounts/Details/5
		
        public async Task<IActionResult> Details(int id)
        {
			//Displays account information as well as all the transactions
			//It shows all the debit and credit
			//Allows transactions to be conducted only on open accounts
			//Tell us wether or not an account can be closed
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Account not found.";
                return RedirectToAction("Index", "Persons");
            }

            var transactions = await _transactionService.GetTransactionsByAccountIdAsync(id);
            var canClose = await _accountService.CanCloseAccountAsync(id);

            var model = new AccountDetailsViewModel
            {
                Account = account,
                Transactions = transactions.ToList(),
                CanClose = canClose,
                CanAddTransaction = account.Status.StatusName == "Open",
                TotalDebits = transactions.Sum(t => t.DebitAmount),
                TotalCredits = transactions.Sum(t => t.CreditAmount)
            };

            return View(model);
        }

        // GET: /Accounts/Create?personId=5
        [HttpGet]
        public async Task<IActionResult> Create(int personId)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var person = await _personService.GetPersonByIdAsync(personId);
            if (person == null)
            {
                TempData["ErrorMessage"] = "Person not found.";
                return RedirectToAction("Index", "Persons");
            }

            ViewBag.PersonName = $"{person.FirstName} {person.Surname}";
            ViewBag.PersonId = personId;

            return View(new AccountViewModel { PersonId = personId });
        }

        // POST: /Accounts/Create
		//Creates new accounts for a person
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountViewModel model)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                var person = await _personService.GetPersonByIdAsync(model.PersonId);
                ViewBag.PersonName = $"{person.FirstName} {person.Surname}";
                return View(model);
            }

            // Checking if the entered account number already exists
            if (await _accountService.AccountNumberExistsAsync(model.AccountNumber))
            {
                ModelState.AddModelError("AccountNumber", "An account with this number already exists.");
                var person = await _personService.GetPersonByIdAsync(model.PersonId);
                ViewBag.PersonName = $"{person.FirstName} {person.Surname}";
                return View(model);
            }

            var account = new Account
            {
                AccountNumber = model.AccountNumber,
                AccountName = model.AccountName,
                PersonId = model.PersonId,
                StatusId = 1, // Open by default
                OutstandingBalance = 0
            };

            var success = await _accountService.CreateAccountAsync(account);

            if (success)
            {
                TempData["SuccessMessage"] = "Account created successfully!";
                return RedirectToAction("Details", "Persons", new { id = model.PersonId });
            }

            ModelState.AddModelError(string.Empty, "Failed to create account. Please try again.");
            var p = await _personService.GetPersonByIdAsync(model.PersonId);
            ViewBag.PersonName = $"{p.FirstName} {p.Surname}";
            return View(model);
        }

        // GET: /Accounts/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Account not found.";
                return RedirectToAction("Index", "Persons");
            }

            var model = new AccountViewModel
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                AccountName = account.AccountName,
                OutstandingBalance = account.OutstandingBalance,
                PersonId = account.PersonId,
                StatusId = account.StatusId,
                PersonName = $"{account.Person.FirstName} {account.Person.Surname}"
            };

            return View(model);
        }

        // POST: /Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AccountViewModel model)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if account number already exists for another account
            if (await _accountService.AccountNumberExistsAsync(model.AccountNumber, model.Id))
            {
                ModelState.AddModelError("AccountNumber", "An account with this number already exists.");
                return View(model);
            }

            // Get existing account to preserve balance and status
            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null)
            {
                TempData["ErrorMessage"] = "Account not found.";
                return RedirectToAction("Index", "Persons");
            }

            var account = new Account
            {
                Id = model.Id,
                AccountNumber = model.AccountNumber,
                AccountName = model.AccountName,
                PersonId = model.PersonId,
                StatusId = existingAccount.StatusId, // Preserve status
                OutstandingBalance = existingAccount.OutstandingBalance // Preserve balance (user can't change it)
            };

            var success = await _accountService.UpdateAccountAsync(account);

            if (success)
            {
                TempData["SuccessMessage"] = "Account updated successfully!";
                return RedirectToAction("Details", new { id = account.Id });
            }

            ModelState.AddModelError(string.Empty, "Failed to update account. Please try again.");
            return View(model);
        }

        // POST: /Accounts/Close/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var canClose = await _accountService.CanCloseAccountAsync(id);
            if (!canClose)
            {
                TempData["ErrorMessage"] = "Cannot close account with non-zero balance.";
                return RedirectToAction("Details", new { id });
            }

            var success = await _accountService.CloseAccountAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Account closed successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to close account.";
            }

            return RedirectToAction("Details", new { id });
        }
    }
}
