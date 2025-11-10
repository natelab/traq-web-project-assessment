using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace traq_web_project_assessment.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A Transaction Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Debit Amount")]
        public decimal DebitAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Credit Amount")]
        public decimal CreditAmount { get; set; } = 0;

        [Required(ErrorMessage = "A Description is required")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Date of Capture")]
        public DateTime CaptureDate { get; set; } = DateTime.Now;

       
        [Required]
        public int AccountId { get; set; } //This is a forgein key

        // Navigation property
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        // Computed property for the purposes of displaying
        [NotMapped]
        public decimal Amount
        {
            get
            {
                return DebitAmount != 0 ? DebitAmount : CreditAmount;
            }
        }

        [NotMapped]
        public string TransactionType
        {
            get
            {
                return DebitAmount != 0 ? "Debit" : "Credit";
            }
        }
    }
}