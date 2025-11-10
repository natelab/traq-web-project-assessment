using System.Diagnostics;
using traq_web_project_assessment.ViewModels;

namespace traq_web_project_assessment.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalPersons { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalTransactions { get; set; }
        public int OpenAccounts { get; set; }
        public int ClosedAccounts { get; set; }
        public decimal TotalBalance { get; set; }

        public List<PersonViewModel> RecentPersons { get; set; } = new List<PersonViewModel>();
        public List<AccountViewModel> RecentAccounts { get; set; } = new List<AccountViewModel>();
        public List<TransactionViewModel> RecentTransactions { get; set; } = new List<TransactionViewModel>();
    }
}

