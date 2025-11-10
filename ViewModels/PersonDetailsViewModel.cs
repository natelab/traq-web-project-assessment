using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.ViewModels
{
    public class PersonDetailsViewModel
    {
        public Person Person { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
        public bool CanDelete { get; set; }
        public bool CanAddAccount { get; set; } = true; // Possible only after a person has been created
    }
}