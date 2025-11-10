using System.ComponentModel.DataAnnotations;

namespace traq_web_project_assessment.ViewModels
{
    public class SearchViewModel
    {
        [Display(Name = "Search By")]
        public string SearchType { get; set; } = "Surname"; // Options: Surname, IdNumber, AccountNumber

        [Display(Name = "Search Term")]
        public string SearchTerm { get; set; }

        // Results
        public List<PersonViewModel> Persons { get; set; } = new List<PersonViewModel>();
        public List<AccountViewModel> Accounts { get; set; } = new List<AccountViewModel>();
    }
}