using System.ComponentModel.DataAnnotations;

namespace traq_web_project_assessment.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Transaction Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; } = DateTime.Today;

        [Display(Name = "Debit Amount")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Range(0, double.MaxValue, ErrorMessage = "Debit amount must be positive")]
        public decimal DebitAmount { get; set; }

        [Display(Name = "Credit Amount")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Range(0, double.MaxValue, ErrorMessage = "Credit amount must be positive")]
        public decimal CreditAmount { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Capture Date")]
        public DateTime CaptureDate { get; set; }

        [Required]
        [Display(Name = "Account")]
        public int AccountId { get; set; }

        // Display properties
        public string? AccountNumber { get; set; }
        public string? PersonName { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransactionType => DebitAmount > 0 ? "Debit" : "Credit";

        [Display(Name = "Amount")]
        public decimal Amount => DebitAmount > 0 ? DebitAmount : CreditAmount;
    }
}