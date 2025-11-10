using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using traq_web_project_assessment.Data;
using traq_web_project_assessment.Models;
using traq_web_project_assessment.ViewModels;

namespace traq_web_project_assessment.Controllers
{
    [Route("home/[action]")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Home or /
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString("Username");

            // Show welcome page if user not logged in
            if (username == null)
                return View("Welcome");

            var dashboardData = await GetDashboardDataAsync();

            return View("Index", dashboardData);
        }

        // This is the Fetching and building of dashboard data
        private async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var model = new DashboardViewModel
            {
                TotalPersons = await _context.Persons.CountAsync(),
                TotalAccounts = await _context.Accounts.CountAsync(),
                TotalTransactions = await _context.Transactions.CountAsync(),
                OpenAccounts = await _context.Accounts
                    .Include(a => a.Status)
                    .CountAsync(a => a.Status.StatusName == "Open"),
                ClosedAccounts = await _context.Accounts
                    .Include(a => a.Status)
                    .CountAsync(a => a.Status.StatusName == "Closed"),
                TotalBalance = await _context.Accounts.SumAsync(a => a.OutstandingBalance),
                RecentPersons = await GetRecentPersonsAsync(),
                RecentAccounts = await GetRecentAccountsAsync(),
                RecentTransactions = await GetRecentTransactionsAsync()
            };

            return model;
        }

        // Retrieve last 5 added persons
        private async Task<List<PersonViewModel>> GetRecentPersonsAsync()
        {
            return await _context.Persons
                .OrderByDescending(p => p.Id)
                .Take(5)
                .Select(p => new PersonViewModel
                {
                    Id = p.Id,
                    IdNumber = p.IdNumber,
                    FirstName = p.FirstName,
                    Surname = p.Surname,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    AccountCount = p.Accounts.Count
                })
                .ToListAsync();
        }

        // Retrieve last 5 created accounts
        private async Task<List<AccountViewModel>> GetRecentAccountsAsync()
        {
            return await _context.Accounts
                .Include(a => a.Person)
                .Include(a => a.Status)
                .OrderByDescending(a => a.Id)
                .Take(5)
                .Select(a => new AccountViewModel
                {
                    Id = a.Id,
                    AccountNumber = a.AccountNumber,
                    AccountName = a.AccountName,
                    OutstandingBalance = a.OutstandingBalance,
                    PersonName = a.Person.FirstName + " " + a.Person.Surname,
                    StatusName = a.Status.StatusName
                })
                .ToListAsync();
        }

        // Retrieve last 5 transactions
        private async Task<List<TransactionViewModel>> GetRecentTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .ThenInclude(a => a.Person)
                .OrderByDescending(t => t.CaptureDate)
                .Take(5)
                .Select(t => new TransactionViewModel
                {
                    Id = t.Id,
                    TransactionDate = t.TransactionDate,
                    DebitAmount = t.DebitAmount,
                    CreditAmount = t.CreditAmount,
                    Description = t.Description,
                    CaptureDate = t.CaptureDate,
                    AccountNumber = t.Account.AccountNumber,
                    PersonName = t.Account.Person.FirstName + " " + t.Account.Person.Surname
                })
                .ToListAsync();
        }

        // Static pages
        [HttpGet]
        public IActionResult About() => View();

        [HttpGet]
        public IActionResult Contact() => View();

        [HttpGet]
        public IActionResult Privacy() => View();

        // Error handler
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
