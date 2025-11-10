﻿using Microsoft.AspNetCore.Mvc;
using traq_web_project_assessment.Models;
using traq_web_project_assessment.Services;
using traq_web_project_assessment.ViewModels;

namespace traq_web_project_assessment.Controllers
{
    [Route("persons/[action]")]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IAccountService _accountService;

        public PersonsController(IPersonService personService, IAccountService accountService)
        {
            _personService = personService;
            _accountService = accountService;
        }

        // GET: /Persons (with optional search)
        [HttpGet]
        public async Task<IActionResult> Index(string searchType, string searchTerm)
        {
            if (!IsUserLoggedIn()) return RedirectToAction("Login", "Auth");

            var persons = await SearchPersonsAsync(searchType, searchTerm);
            var viewModels = persons.Select(MapToPersonViewModel).ToList();

            if (searchType == "AccountNumber" && ViewBag.SearchedAccount != null)
            {
                // keep ViewBag info for view display
            }

            return View(viewModels);
        }

        // GET: /Persons/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!IsUserLoggedIn()) return RedirectToAction("Login", "Auth");

            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                TempData["ErrorMessage"] = "Person not found.";
                return RedirectToAction("Index");
            }

            var accounts = await _accountService.GetAccountsByPersonIdAsync(id);
            var canDelete = await _personService.CanDeletePersonAsync(id);

            var model = new PersonDetailsViewModel
            {
                Person = person,
                Accounts = accounts.ToList(),
                CanDelete = canDelete,
                CanAddAccount = true
            };

            return View(model);
        }

        // GET: /Persons/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsUserLoggedIn()) return RedirectToAction("Login", "Auth");
            return View();
        }

        // POST: /Persons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonViewModel model)
        {
            if (!IsUserLoggedIn()) return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid) return View(model);

            if (await _personService.IdNumberExistsAsync(model.IdNumber))
            {
                ModelState.AddModelError("IdNumber", "A person with this ID Number already exists.");
                return View(model);
            }

            var person = MapToPerson(model);
            var success = await _personService.CreatePersonAsync(person);

            if (success)
            {
                TempData["SuccessMessage"] = "Person created successfully!";
                return RedirectToAction("Details", new { id = person.Id });
            }

            ModelState.AddModelError(string.Empty, "Failed to create person. Please try again.");
            return View(model);
        }

        // GET: /Persons/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsUserLoggedIn()) return RedirectToAction("Login", "Auth");

            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                TempData["ErrorMessage"] = "Person not found.";
                return RedirectToAction("Index");
            }

            return View(MapToPersonViewModel(person));
        }

        // POST: /Persons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PersonViewModel model)
        {
            if (!IsUserLoggedIn()) return RedirectToAction("Login", "Auth");
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            if (await _personService.IdNumberExistsAsync(model.IdNumber, model.Id))
            {
                ModelState.AddModelError("IdNumber", "A person with this ID Number already exists.");
                return View(model);
            }

            var person = MapToPerson(model);
            var success = await _personService.UpdatePersonAsync(person);

            if (success)
            {
                TempData["SuccessMessage"] = "Person updated successfully!";
                return RedirectToAction("Details", new { id = person.Id });
            }

            ModelState.AddModelError(string.Empty, "Failed to update person. Please try again.");
            return View(model);
        }

        // GET: /Persons/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsUserLoggedIn()) return RedirectToAction("Login", "Auth");

            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                TempData["ErrorMessage"] = "Person not found.";
                return RedirectToAction("Index");
            }

            if (!await _personService.CanDeletePersonAsync(id))
            {
                TempData["ErrorMessage"] = "Cannot delete person with open accounts.";
                return RedirectToAction("Details", new { id });
            }

            return View(MapToPersonViewModel(person));
        }

        // POST: /Persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsUserLoggedIn()) return RedirectToAction("Login", "Auth");

            var success = await _personService.DeletePersonAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Person deleted successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Failed to delete person. Person may have open accounts.";
            return RedirectToAction("Details", new { id });
        }

        // ===== Helper Methods =====

        private bool IsUserLoggedIn() => HttpContext.Session.GetString("Username") != null;

        private Person MapToPerson(PersonViewModel model) => new()
        {
            Id = model.Id,
            IdNumber = model.IdNumber,
            FirstName = model.FirstName,
            Surname = model.Surname,
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email
        };

        private PersonViewModel MapToPersonViewModel(Person person) => new()
        {
            Id = person.Id,
            IdNumber = person.IdNumber,
            FirstName = person.FirstName,
            Surname = person.Surname,
            Address = person.Address,
            PhoneNumber = person.PhoneNumber,
            Email = person.Email,
            AccountCount = person.Accounts.Count
        };

        private async Task<IEnumerable<Person>> SearchPersonsAsync(string searchType, string searchTerm)
        {
            IEnumerable<Account> accounts = new List<Account>();

            if (string.IsNullOrWhiteSpace(searchTerm))
                return await _personService.GetAllPersonsAsync();

            ViewBag.SearchType = searchType ?? "Surname";
            ViewBag.SearchTerm = searchTerm;

            return searchType switch
            {
                "IdNumber" => (await _personService.GetPersonByIdNumberAsync(searchTerm)) is Person person
                    ? new List<Person> { person }
                    : new List<Person>(),

                "Surname" => await _personService.SearchPersonsBySurnameAsync(searchTerm),

                "AccountNumber" => await HandleAccountSearchAsync(searchTerm),

                _ => await _personService.GetAllPersonsAsync()
            };
        }

        private async Task<IEnumerable<Person>> HandleAccountSearchAsync(string accountNumber)
        {
            var account = await _accountService.GetAccountByAccountNumberAsync(accountNumber);
            if (account == null) return new List<Person>();

            ViewBag.SearchedAccount = account;
            return new List<Person> { account.Person };
        }
    }
}
