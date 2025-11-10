using System.ComponentModel.DataAnnotations;

namespace traq_web_project_assessment.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account Number is required")]
        [StringLength(20)]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Account Name is required")]
        [StringLength(100)]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Display(Name = "Outstanding Balance")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal OutstandingBalance { get; set; }

        [Required]
        [Display(Name = "Person")]
        public int PersonId { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }


        // These are the Display properties
        public string? PersonName { get; set; }
        public string? StatusName { get; set; }
        public int TransactionCount { get; set; }
        public bool CanClose { get; set; }
        public bool IsClosed { get; set; }
    }
}