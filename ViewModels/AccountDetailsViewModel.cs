using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.ViewModels
{
    public class AccountDetailsViewModel
    {
        public Account Account { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public bool CanClose { get; set; }
        public bool CanAddTransaction { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
    }
}